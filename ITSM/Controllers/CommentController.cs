using ITSM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITSM.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommenctService _commentService;

        public CommentController(ICommenctService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public IActionResult AddComment(int projectId, string message)
        {
            var result = _commentService.AddComment(projectId, message, User);

            if(result) TempData["success"] = "Comment has been added.";
            else TempData["error"] = "Something went wrong while creating your comment.";

            return RedirectToAction("Details", "Project", new { id = projectId });
        }

        [HttpPost]
        public IActionResult DeleteComment(int projectId, int commentId)
        {
            var result = _commentService.DeleteComment(commentId, User);

            if(result) TempData["success"] = "Comment has been removed.";
            else TempData["error"] = "Something went wrong while creating your comment.";

            return RedirectToAction("Details", "Project", new { id = projectId });
        }
    }
}
