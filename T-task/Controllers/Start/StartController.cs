using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T_task.Models;

namespace T_task.Controllers.Start
{
    public class StartController : Controller
    {
        private Matrix _matrix = new Matrix();
        private int _variables;
        private int _restrictions;

        public ActionResult Index()
        {
            return View(_matrix);
        }

        [HttpPost]
        public ActionResult Index(Matrix matrix)
        {
            _matrix = matrix;
            return RedirectToAction("Matrix", _matrix);
        }

        public ActionResult Matrix(string variables, string restrictions)
        {
            _matrix.Variables = Convert.ToInt32(variables);
            _matrix.Restrictions = Convert.ToInt32(restrictions);

            _matrix.CMatrix = new double[_matrix.Restrictions + 1][];

            for (int i = 0; i < _matrix.Restrictions + 1; i++)
            {
                _matrix.CMatrix[i] = new double[_matrix.Variables + 1];
            }

            for (int i = 0; i < _matrix.Restrictions; i++)
            {
                for (int j = 0; j < _matrix.Variables; j++)
                {
                    _matrix.CMatrix[i][j] = 1;
                }
            }

            return View(_matrix);
        }

        [HttpPost]
        public ActionResult Matrix(Matrix matrix)
        {
            ViewBag.Matrix = matrix.NorthWest(matrix);
            return View("Result");
        }
    }
}