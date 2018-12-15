using System;
using System.Web.Mvc;
using T_task.Models;

namespace T_task.Controllers
{
    public class MaxHungarianController : Controller
    {
        private Hungarian _matrix = new Hungarian();

        public ActionResult VarsRestr()
        {
            ViewBag.Selected = "MaxHungarian";
            _matrix.Variables = 4;
            _matrix.Restrictions = 4;

            return View(_matrix);
        }

        [HttpPost]
        public ActionResult VarsRestr(Hungarian matrix)
        {
            _matrix = matrix;
            return RedirectToAction("MaxHungarian", _matrix);
        }

        public ActionResult MaxHungarian(string variables, string restrictions)
        {
            ViewBag.Selected = "MaxHungarian";

            _matrix.Variables = Convert.ToInt32(variables);
            _matrix.Restrictions = Convert.ToInt32(restrictions);

            _matrix.CMatrix = new double[_matrix.Restrictions + 1][];

            for (int i = 0; i < _matrix.Restrictions + 1; i++)
            {
                _matrix.CMatrix[i] = new double[_matrix.Variables + 1];
            }

            //my task
            _matrix.CMatrix[0][0] = 90;
            _matrix.CMatrix[0][1] = 75;
            _matrix.CMatrix[0][2] = 75;
            _matrix.CMatrix[0][3] = 80;

            _matrix.CMatrix[1][0] = 35;
            _matrix.CMatrix[1][1] = 85;
            _matrix.CMatrix[1][2] = 55;
            _matrix.CMatrix[1][3] = 65;

            _matrix.CMatrix[2][0] = 125;
            _matrix.CMatrix[2][1] = 95;
            _matrix.CMatrix[2][2] = 90;
            _matrix.CMatrix[2][3] = 105;

            _matrix.CMatrix[3][0] = 45;
            _matrix.CMatrix[3][1] = 110;
            _matrix.CMatrix[3][2] = 95;
            _matrix.CMatrix[3][3] = 115;

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
        public ActionResult MaxHungarian(Hungarian matrix)
        {
            ViewBag.Selected = "MaxHungarian";

            ViewBag.Result = matrix.MaxHungarianMethod();
            ViewBag.Matrix = matrix.TempMatrix;

            return View("MaxHungarianResult");
        }
    }
}