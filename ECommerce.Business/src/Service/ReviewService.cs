using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;

namespace ECommerceBusiness;
public class ReviewService : IReviewService
{
    private readonly IReviewRepo _repo;
    private IMapper _mapper;

    public ReviewService(IReviewRepo repo,IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public bool Cancel(Guid id)
    {
        if (id == Guid.Empty)
        {
           throw new Exception("bad request");
        }
       return _repo.Delete(id);
    }

    public ReviewReadDTO Create(ReviewCreateDTO review)
    {
        if(review == null)
        {
            throw new Exception("bad request");
        }

        try
        {
            var newReview = _mapper.Map<ReviewCreateDTO,Review>(review);
            var result =_repo.CreateNew(newReview);
            return _mapper.Map<Review, ReviewReadDTO>(result);
        }
        catch(Exception)
        {
           throw;
        }
      
    }

    public IEnumerable<ReviewReadDTO> GetAll(BaseQueryParameter options)
    {
        return _repo.GetAll(options).Select(r=>_mapper.Map<Review,ReviewReadDTO>(r));
    }

    public ReviewReadDTO GetById(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new Exception("bad request");
        }
        try
        {
            var target = _repo.GetById(id);
            if(target != null)
            {
                var mappedResult = _mapper.Map<ReviewReadDTO>(target);
                return mappedResult;
            }
            throw new Exception("not found");
        }
        catch(Exception)
        {
            throw;
        }
    }

    public ReviewReadDTO Update(Guid id, ReviewUpdateDTO updates)
    {
        if (id == Guid.Empty || updates == null)
        {
            throw new Exception("bad request");
        }
        
        try
        {
            var targetReview =_repo.GetById(id);
            if (targetReview == null)
            {
                throw new Exception("not found");
            }
            targetReview.Feedback = updates.Feedback;
            return _mapper.Map<ReviewReadDTO>(targetReview);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
