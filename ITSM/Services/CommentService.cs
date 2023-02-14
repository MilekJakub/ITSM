using ITSM.Data;
using ITSM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITSM.Services
{
    public interface ICommenctService
    {
        bool AddComment(int projectId, string message, ClaimsPrincipal claims);
        bool DeleteComment(int commentId, ClaimsPrincipal claims);
    }

    public class CommentService : ICommenctService
    {
        private readonly BoardsContext _dbContext;

        public CommentService(BoardsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddComment(int id, string message, ClaimsPrincipal claims)
        {
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier);

            if(userId == null)
                return false;

            var comment = new Comment()
            {
                Message = message,
                ProjectId = id,
                UserId = userId.Value,
            };

            try
            {
                _dbContext.Comments.Add(comment);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool DeleteComment(int commentId, ClaimsPrincipal claims)
        {
            var comment = _dbContext.Comments.FirstOrDefault(x => x.Id == commentId);
            
            if (comment == null)
                return false;

            var userId = claims.FindFirst(ClaimTypes.NameIdentifier);

            if(userId == null || comment.UserId != userId.Value)
                return false;

            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
