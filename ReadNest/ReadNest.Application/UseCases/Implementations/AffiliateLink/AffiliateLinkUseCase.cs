using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Models.Requests.AffiliateLink;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.AffiliateLink;
using ReadNest.Application.Validators.AffiliateLink;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.AffiliateLink
{
    public class AffiliateLinkUseCase : IAffiliateLinkUseCase
    {
        private readonly IAffiliateLinkRepository _affiliateLinkRepository;
        private readonly IBookRepository _bookRepository;
        private readonly CreateAffiliateLinkRequestValidator _validator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="affiliateLinkRepository"></param>
        /// <param name="bookRepository"></param>
        /// <param name="validator"></param>
        public AffiliateLinkUseCase(IAffiliateLinkRepository affiliateLinkRepository, IBookRepository bookRepository, CreateAffiliateLinkRequestValidator validator)
        {
            _affiliateLinkRepository = affiliateLinkRepository;
            _bookRepository = bookRepository;
            _validator = validator;
        }

        public async Task<ApiResponse<string>> CreateAsync(Guid bookId, CreateAffiliateLinkRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);

            var isExistBook = await _bookRepository.GetByIdAsync(bookId) != null;
            if (!isExistBook)
            {
                return ApiResponse<string>.Fail(MessageId.E0003);
            }

            request.AffiliateLinkRequests.ForEach(async item =>
            {
                var affiliateLink = new Domain.Entities.AffiliateLink
                {
                    BookId = bookId,
                    PartnerName = item.PartnerName,
                    Link = item.AffiliateLink
                };

                _ = await _affiliateLinkRepository.AddAsync(affiliateLink);
            });

            await _affiliateLinkRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok(string.Empty);
        }

        public async Task<ApiResponse<string>> UpdateAsync(Guid bookId, UpdateAffiliateLinkRequest request)
        {
            var book = (await _bookRepository.FindWithIncludeAsync(
                predicate: query => !query.IsDeleted && query.Id == bookId,
                include: query => query.Include(x => x.AffiliateLinks),
                asNoTracking: false))
                .FirstOrDefault();

            if(book == null)
            {
                return ApiResponse<string>.Fail(MessageId.E0005);
            }

            var deletedAffiliateLinks = book.AffiliateLinks.ToDictionary(x => x.Id);

            foreach(var affiliate in request.UpdateAffiliateLinks)
            {
                if(affiliate.id.HasValue && deletedAffiliateLinks.TryGetValue(affiliate.id.Value, out var affiliateLinkInDb))
                {
                    deletedAffiliateLinks.Remove(affiliate.id.Value);
                    affiliateLinkInDb.PartnerName = affiliate.partnerName;
                    affiliateLinkInDb.Link = affiliate.affilateLink;
                }

                if(!affiliate.id.HasValue)
                {
                    var addedAffiliateLink = new Domain.Entities.AffiliateLink
                    {
                        PartnerName = affiliate.partnerName,
                        Link = affiliate.affilateLink,
                    };

                    book.AffiliateLinks.Add(addedAffiliateLink);
                }
            }

            foreach(var item in deletedAffiliateLinks.Values)
            {
                book.AffiliateLinks.Remove(item);
            }

            await _bookRepository.UpdateAsync(book);
            await _bookRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(string.Empty);
        }
    }
}
