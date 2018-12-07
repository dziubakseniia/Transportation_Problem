using System;
using System.Web.Mvc;
using T_task.Models;

namespace T_task.Controllers
{
    public class LowCostController : Controller
    {
        private Matrix _matrix = new Matrix();

        public ActionResult VarsRestr()
        {
            ViewBag.Selected = "LowCost";
            _matrix.Variables = 4;
            _matrix.Restrictions = 3;

            return View(_matrix);
        }

        [HttpPost]
        public ActionResult VarsRestr(Matrix matrix)
        {
            _matrix = matrix;
            return RedirectToAction("LowCost", _matrix);
        }

        public ActionResult LowCost(string variables, string restrictions)
        {
            ViewBag.Selected = "LowCost";

            _matrix.Variables = Convert.ToInt32(variables);
            _matrix.Restrictions = Convert.ToInt32(restrictions);

            _matrix.CMatrix = new double[_matrix.Restrictions + 1][];

            for (int i = 0; i < _matrix.Restrictions + 1; i++)
            {
                _matrix.CMatrix[i] = new double[_matrix.Variables + 1];
            }

            _matrix.CMatrix[0][0] = 4;
            _matrix.CMatrix[0][1] = 4;
            _matrix.CMatrix[0][2] = 2;
            _matrix.CMatrix[0][3] = 5;
            _matrix.CMatrix[0][4] = 150;

            _matrix.CMatrix[1][0] = 5;
            _matrix.CMatrix[1][1] = 3;
            _matrix.CMatrix[1][2] = 1;
            _matrix.CMatrix[1][3] = 2;
            _matrix.CMatrix[1][4] = 60;

            _matrix.CMatrix[2][0] = 2;
            _matrix.CMatrix[2][1] = 1;
            _matrix.CMatrix[2][2] = 4;
            _matrix.CMatrix[2][3] = 2;
            _matrix.CMatrix[2][4] = 80;

            _matrix.CMatrix[3][0] = 110;
            _matrix.CMatrix[3][1] = 40;
            _matrix.CMatrix[3][2] = 60;
            _matrix.CMatrix[3][3] = 80;
            _matrix.CMatrix[3][4] = 0;


            //test task
            //_matrix.CMatrix[0][0] = 1;
            //_matrix.CMatrix[0][1] = 2;
            //_matrix.CMatrix[0][2] = 9;
            //_matrix.CMatrix[0][3] = 7;
            //_matrix.CMatrix[0][4] = 60;

            //_matrix.CMatrix[1][0] = 3;
            //_matrix.CMatrix[1][1] = 4;
            //_matrix.CMatrix[1][2] = 1;
            //_matrix.CMatrix[1][3] = 5;
            //_matrix.CMatrix[1][4] = 55;

            //_matrix.CMatrix[2][0] = 6;
            //_matrix.CMatrix[2][1] = 4;
            //_matrix.CMatrix[2][2] = 8;
            //_matrix.CMatrix[2][3] = 3;
            //_matrix.CMatrix[2][4] = 40;

            //_matrix.CMatrix[3][0] = 2;
            //_matrix.CMatrix[3][1] = 3;
            //_matrix.CMatrix[3][2] = 3;
            //_matrix.CMatrix[3][3] = 1;
            //_matrix.CMatrix[3][4] = 35;

            //_matrix.CMatrix[4][0] = 70;
            //_matrix.CMatrix[4][1] = 5;
            //_matrix.CMatrix[4][2] = 45;
            //_matrix.CMatrix[4][3] = 70;
            //_matrix.CMatrix[4][4] = 0;

            return View(_matrix);
        }

        [HttpPost]
        public ActionResult LowCost(Matrix matrix)
        {
            ViewBag.Selected = "LowCost";

            matrix.Variables += 1;
            matrix.Restrictions += 1;

            ViewBag.Matrix = matrix.LowCost(matrix);

            return View("LowCostResult");
        }
    }
}