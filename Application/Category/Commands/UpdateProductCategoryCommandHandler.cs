using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Category.Commands
{
    public class UpdateProductCategoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.ProductCategories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
                return false;

            category.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
