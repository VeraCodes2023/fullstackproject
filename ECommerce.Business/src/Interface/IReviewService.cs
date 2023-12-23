using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerceBusiness;
public interface IReviewService
{
    IEnumerable<ReviewReadDTO> GetAll(BaseQueryParameter options);
    ReviewReadDTO GetById(Guid id);
    ReviewReadDTO Create(ReviewCreateDTO  review);
    bool Cancel(Guid id);
    ReviewReadDTO Update(Guid id, ReviewUpdateDTO review);
}
