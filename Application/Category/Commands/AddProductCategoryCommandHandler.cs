using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Category.Commands
{
    public class AddProductCategoryCommandHandler : IRequestHandler<AddProductCategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public AddProductCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(AddProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new ProductCategory
            {
                Name = request.Name
            };

            _context.ProductCategories.Add(category);
             await _context.SaveChangesAsync(cancellationToken);
            

            return category.Id;
        }
    }
}
