using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core;
public interface IReviewRepo
{
    IEnumerable<Review> GetAll(BaseQueryParameter options);
    Review CreateNew(Review review);// cutomer auth
    Review GetById(Guid id); // customer auth
    Review Update(Guid reviewId, Review update); // customer auth
    bool Delete(Guid id); // customer auth
    
}
