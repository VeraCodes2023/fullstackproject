using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using ECommerce.Business;
using ECommerceWebAPI;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebAPI;

public class ReviewRepo : IReviewRepo
{
    protected DbSet<Review> _feedbacks;
    private DatabaseContext _database;
    public ReviewRepo(DatabaseContext database)
    {
        _feedbacks = database.Reviews;
        _database = database;
    }
    public Review CreateNew(Review review)
    {
        _feedbacks.Add(review);
        _database.SaveChanges();
        return review;
    }

    public bool Delete(Guid id)
    {
        var foundReview = _feedbacks.Find(id);
        if(foundReview is null)
        {
            return false;
        }
        return true;
    }

    public IEnumerable<Review> GetAll(BaseQueryParameter options)
    {
            return _feedbacks.AsNoTracking()
            .Skip(0)
            .Take(100);
    }

    public Review GetById(Guid id)
    {
        var foundReview = _feedbacks.Find(id);
        return foundReview!;
    }

    public Review Update(Guid reviewId, Review update)
    {
        var existingReview = _feedbacks.Find(reviewId);
        existingReview!.Feedback = update.Feedback;
        _feedbacks.Update(existingReview);
        _database.SaveChanges();
        return existingReview;
    }
}
