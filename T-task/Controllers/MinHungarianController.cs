using System;
using System.Web.Mvc;
using T_task.Models;

namespace T_task.Controllers
{
    public class MinHungarianController : Controller
    {
        private Hungarian _matrix = new Hungarian();

        public ActionResult VarsRestr()
        {
            ViewBag.Selected = "MinHungarian";
            _matrix.Variables = 5;
            _matrix.Restrictions = 5;

            return View(_matrix);
        }

        [HttpPost]
        public ActionResult VarsRestr(Hungarian matrix)
        {
            _matrix = matrix;
            return RedirectToAction("MinHungarian", _matrix);
        }

        public ActionResult MinHungarian(string variables, string restrictions)
        {
            ViewBag.Selected = "MinHungarian";

            _matrix.Variables = Convert.ToInt32(variables);
            _matrix.Restrictions = Convert.ToInt32(restrictions);

            _matrix.CMatrix = new double[_matrix.Restrictions + 1][];

            for (int i = 0; i < _matrix.Restrictions + 1; i++)
            {
                _matrix.CMatrix[i] = new double[_matrix.Variables + 1];
            }

            //my task
            _matrix.CMatrix[0][0] = 7;
            _matrix.CMatrix[0][1] = 4;
            _matrix.CMatrix[0][2] = 8;
            _matrix.CMatrix[0][3] = 7;
            _matrix.CMatrix[0][4] = 4;

            _matrix.CMatrix[1][0] = 10;
            _matrix.CMatrix[1][1] = 8;
            _matrix.CMatrix[1][2] = 6;
            _matrix.CMatrix[1][3] = 10;
            _matrix.CMatrix[1][4] = 9;

            _matrix.CMatrix[2][0] = 5;
            _matrix.CMatrix[2][1] = 6;
            _matrix.CMatrix[2][2] = 7;
            _matrix.CMatrix[2][3] = 5;
            _matrix.CMatrix[2][4] = 10;

            _matrix.CMatrix[3][0] = 9;
            _matrix.CMatrix[3][1] = 9;
            _matrix.CMatrix[3][2] = 10;
            _matrix.CMatrix[3][3] = 9;
            _matrix.CMatrix[3][4] = 8;

            _matrix.CMatrix[4][0] = 8;
            _matrix.CMatrix[4][1] = 10;
            _matrix.CMatrix[4][2] = 9;
            _matrix.CMatrix[4][3] = 8;
            _matrix.CMatrix[4][4] = 7;


            //test task
            //_matrix.CMatrix[0][0] = 2;
            //_matrix.CMatrix[0][1] = 4;
            //_matrix.CMatrix[0][2] = 1;
            //_matrix.CMatrix[0][3] = 3;
            //_matrix.CMatrix[0][4] = 3;

            //_matrix.CMatrix[1][0] = 1;
            //_matrix.CMatrix[1][1] = 5;
            //_matrix.CMatrix[1][2] = 4;
            //_matrix.CMatrix[1][3] = 1;
            //_matrix.CMatrix[1][4] = 2;

            //_matrix.CMatrix[2][0] = 3;
            //_matrix.CMatrix[2][1] = 5;
            //_matrix.CMatrix[2][2] = 2;
            //_matrix.CMatrix[2][3] = 2;
            //_matrix.CMatrix[2][4] = 4;

            //_matrix.CMatrix[3][0] = 1;
            //_matrix.CMatrix[3][1] = 4;
            //_matrix.CMatrix[3][2] = 3;
            //_matrix.CMatrix[3][3] = 1;
            //_matrix.CMatrix[3][4] = 4;

            //_matrix.CMatrix[4][0] = 3;
            //_matrix.CMatrix[4][1] = 2;
            //_matrix.CMatrix[4][2] = 5;
            //_matrix.CMatrix[4][3] = 3;
            //_matrix.CMatrix[4][4] = 5;

            return View(_matrix);
        }

        [HttpPost]
        public ActionResult MinHungarian(Hungarian matrix)
        {
            ViewBag.Selected = "MinHungarian";

            ViewBag.Result = matrix.MinHungarianMethod();
            ViewBag.Matrix = matrix.TempMatrix;

            return View("MinHungarianResult");
        }
    }
}