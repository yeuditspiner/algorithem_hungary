using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace hungary_togit
{
    public partial class Form1 : Form
    {


        List<int> finalResult = new List<int>();
        List<int> optimalR = new List<int>();//list of rows without allocated
        List<int> optimalC = new List<int>();
        Dictionary<int, List<int>> Drows = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> Dcoulnms = new Dictionary<int, List<int>>();
        readonly Dictionary<int, List<int>> Dcoulnmsmarked = new Dictionary<int, List<int>>();

        public int Lines = 0;
        public int key = 0;
        Dictionary<int, Point> dNon;//מילון אברים "אסורים"
        Dictionary<int, Point> d;//מילון אברים "מותרים"
        int[] linesoptimal;//מערך שורות מוקצות
        int[] linesoNotptimal;//מערך שורות   לא מוקצות
        int[] cells;//מערך עמודות
        public List<int> lstNonOptimalRow = new List<int>();//list of rows without plalcment
        public List<int> lstOptimalRow = new List<int>();//list of rows with allocated
        public List<int> lstNonOptimalCoulnm = new List<int>();//list of coulnm without allocated



        int[,] globalmat = new int[14, 14]  {
         { 240 ,140 ,225, 140 ,206, 339 ,339 ,339, 206, 215,215, 0, 0, 0 },

        { 254 ,0 ,37, 0 ,43, 58 ,58, 58 ,43 ,38,38, 67, 67, 67 },

           { 0 ,107 ,158, 107, 151 ,206, 206, 206, 151 ,182 ,182, 0, 0 ,0 },

          { 0, 253, 245 ,253, 304 ,235 ,235, 235, 304, 402 ,402, 220, 220, 220 },

           { 300, 27, 56, 2, 11, 0 ,0 ,0, 11, 0, 0, 227 ,227, 227 },

         { 300, 0, 145, 0, 0 ,230 ,230, 230, 0, 284 ,284 ,227 ,227 ,227 },

           { 80, 12,0, 188, 120, 176 ,269 ,269, 269, 176,193 ,193, 0, 0  },

           { 207, 0 ,0 ,0 ,151, 143, 143, 143, 151,96, 96, 167, 167, 167 },

         {  229,9, 95, 9 ,0 ,110, 110, 110, 0, 159 ,159, 22 ,22 ,22 },

          { 147, 0 ,40, 0,148, 221 ,221 ,221 ,148,171 , 171, 0 ,0 ,0 },

          { 240 ,133, 203 ,133 ,187, 28, 282 ,282, 187, 215 ,215 ,0 ,0 ,0 },

          { 189, 3, 0, 3, 94, 58 ,58, 58 ,94, 192, 192 ,16 ,16 ,16 },

          { 367 ,87 ,36 ,87, 153 ,0, 0, 0 ,153, 379, 379, 200, 200, 200 },

         { 194, 0, 82, 0, 11 ,115, 115 ,115, 11, 112 ,112 ,127, 127 ,127 } };

        int[,] matOrginal = new int[14, 14] {
              { 240 ,140 ,225, 140 ,206, 339 ,339 ,339, 206, 215,215, 0, 0, 0 },

        { 254 ,0 ,37, 0 ,43, 58 ,58, 58 ,43 ,38,38, 67, 67, 67 },

           { 0 ,107 ,158, 107, 151 ,206, 206, 206, 151 ,182 ,182, 0, 0 ,0 },

          { 0, 253, 245 ,253, 304 ,235 ,235, 235, 304, 402 ,402, 220, 220, 220 },

           { 300, 27, 56, 2, 11, 0 ,0 ,0, 11, 0, 0, 227 ,227, 227 },

         { 300, 0, 145, 0, 0 ,230 ,230, 230, 0, 284 ,284 ,227 ,227 ,227 },

           { 80, 12,0, 188, 120, 176 ,269 ,269, 269, 176,193 ,193, 0, 0  },

           { 207, 0 ,0 ,0 ,151, 143, 143, 143, 151,96, 96, 167, 167, 167 },

         {  229,9, 95, 9 ,0 ,110, 110, 110, 0, 159 ,159, 22 ,22 ,22 },

          { 147, 0 ,40, 0,148, 221 ,221 ,221 ,148,171 , 171, 0 ,0 ,0 },

          { 240 ,133, 203 ,133 ,187, 28, 282 ,282, 187, 215 ,215 ,0 ,0 ,0 },

          { 189, 3, 0, 3, 94, 58 ,58, 58 ,94, 192, 192 ,16 ,16 ,16 },

          { 367 ,87 ,36 ,87, 153 ,0, 0, 0 ,153, 379, 379, 200, 200, 200 },

         { 194, 0, 82, 0, 11 ,115, 115 ,115, 11, 112 ,112 ,127, 127 ,127 } };




        public Form1()
        {
            InitializeComponent();
            Hungary(globalmat);
            Dictionary<int, List<int>> d = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> dr = LstFinalResult(globalmat);

        }
        public bool IsFind(List<int> l, Dictionary<int, List<int>> dr) => dr.Values.Contains(l);
        //פעולה המקבלת מטריצה ומחזירה את תת המטריצה שלה
        public int[,] GetSubmatrix(int[,] mat, int row, int coulnm)
        {
            int[,] mat1 = new int[mat.GetLength(0) - 1, mat.GetLength(0) - 1];
            int r = 0, c = 0;
            //הכנת התת מטריצה
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(0); j++)
                {
                    if (i != row && j != coulnm)
                    {
                        if (r < mat1.GetLength(0) && c < mat1.GetLength(0))
                            mat1[r, c] = mat[i, j];
                        c++;
                        if (c == mat.GetLength(0) - 1 && r + 1 < mat.GetLength(0) - 1)
                        {
                            c = 0;
                            r++;
                        }

                    }
                }
            }
            return mat1;
        }
        //מחזירה מילון תוצאות
        public Dictionary<int, List<int>> LstFinalResult(int[,] mat)
        {
            Dictionary<int, List<int>> d = new Dictionary<int, List<int>>();
            int count = 0;
            int[,] m;
            int[,] Orginalm;
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                if (mat[0, i] == 0)
                {
                    m = GetSubmatrix(mat, 0, i);//תת מטריצה נוכחית
                    int firstInLst = matOrginal[0, i];
                    Orginalm = GetSubmatrix(matOrginal, 0, i);//תת מטריצה מקורית נכון
                    if (Rec(m))
                    {
                        m = GetSubmatrix(mat, 0, i);
                        List<List<int>> slo = final(m, Orginalm, firstInLst, d);
                        if (slo.Count() > 0)
                            foreach (var item in slo.ToList())
                                d.Add(count++, item);
                    }
                }
            }
            return d;
        }

        /// <summary>
        /// Get a dictionary of result lists and returns the list with the lowest standard deviation
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public List<int> getFinalSlution(Dictionary<int, List<int>> resultDictionary)
        {
            double min = 100000;
            int min_i = 0;
            foreach (KeyValuePair<int, List<int>> entry in resultDictionary)
            {
                double sd = StandardDeviation(entry.Value);
                if (sd < min)
                {
                    min = sd;
                    min_i = entry.Key;
                }
            }
            return resultDictionary[min_i];
        }
        /// <summary>
        /// Get a list and returns its standard deviation
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public double StandardDeviation(List<int> lst)
        {
            int sum = 0;
            double avg = 0, standard_deviation = 0, sd = 0;

            foreach (var item in lst)
            {
                sum += Convert.ToInt32(item);
            }
            avg = sum / lst.Count();
            foreach (var item in lst)
            {
                sd += Math.Pow((double)(item - avg), 2);

            }
            double x = (double)1 / (double)lst.Count();
            x *= sd;
            standard_deviation = Math.Sqrt(x);
            return standard_deviation;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //חיסור לשורה לא מכוסה 
        public void UnCoveredLine(int mininmat)
        {
            for (int i = 0; i < globalmat.GetLength(0); i++)
            {
                for (int j = 0; j < globalmat.GetLength(0); j++)
                {
                    // if (!IsInDRow(globalmat[i, j], i))
                    if (!isrowInD(i) && !isCoulnmInD(j))
                        globalmat[i, j] -= mininmat;
                }
            }
        }
        //הוספה לעמודה מכוסה
        public void CoverColunm(int mininmat)
        {
            for (int i = 0; i < globalmat.GetLength(0); i++)
            {
                for (int j = 0; j < globalmat.GetLength(0); j++)
                {
                    //הוספה לעמודה  מכוסה
                    // if (isCoulnmInD(j) || IsInDCouknm(globalmat[i, j], j) && !isrowInD(i))
                    if (isCoulnmInD(j) || IsInDCouknm(globalmat[i, j], j) && !isrowInD(i))
                        globalmat[i, j] += mininmat;
                }
            }
        }
        ////הוספה למשהו שמכוסה פעמיים
        public void Addtodouble(int mininmat)
        {
            for (int i = 0; i < globalmat.GetLength(0); i++)
            {
                for (int j = 0; j < globalmat.GetLength(0); j++)
                {
                    if (isCoulnmInD(j) && isrowInD(i))
                        globalmat[i, j] += mininmat;
                }
            }
        }


        //פונקציה רקורסיבית אשר מחזירה עבור תת מטריצה האם הוא 0
        public bool Rec(int[,] mat)
        {
            if (mat.GetLength(0) == 0)
            {

                return mat[0, 0] == 0;
            }
            if (mat.GetLength(0) == 1)
            {
                if (mat[0, 0] == 0)
                    return true;
                return false;
            }
            else
            {
                int[,] submatrix;
                int zeros = ZeroInArr(mat, 0, true);
                if (zeros == 0)
                    return false;
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    if (mat[0, i] == 0)
                    {
                        submatrix = GetSubmatrix(mat, 0, i);
                        if (Rec(submatrix))
                            return true;
                    }
                }
            }
            return true;
        }

        private int ZeroInArr(int[,] mat, int line, bool v2)
        {
            int count = 0;
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                if (mat[line, i] == 0)
                    count++;

            }
            return count;
        }


        //פעולה המקבלת מטריצה ומחזירה את תת המטריצה שלה
        public int[,] FinalResult(int[,] mat, int row, int coulnm)
        {
            int[,] mat1 = new int[mat.GetLength(0) - 1, mat.GetLength(0) - 1];
            int r = 0, c = 0;
            //הכנת התת מטריצה
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(0); j++)
                {
                    if (i != row && j != coulnm)
                    {
                        mat1[r, c++] = mat[i, j];
                        if (c == mat.GetLength(0) - 1)
                        {
                            c = 0;
                            r++;
                        }
                    }
                }
            }
            return mat1;
        }
        public int[,] FinalResultOrginal(int[,] mat, int row, int coulnm)
        {
            int[,] mat1 = new int[mat.GetLength(0) - 1, mat.GetLength(0) - 1];
            int r = 0, c = 0;
            //הכנת התת מטריצה
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(0); j++)
                {
                    if (i != row && j != coulnm)
                    {
                        mat1[r, c++] = mat[i, j];
                        if (c == mat.GetLength(0) - 1)
                        {
                            c = 0;
                            r++;
                        }
                    }

                }
            }
            return mat1;
        }
        /// פונקציה המקבלת מטריצה לאחר ביצוע "הונגרי" ומחזירה את כל הפתרונות האפשריים של תת מטריצה מסוים
        /// </summary>
        /// <param name = "mat" ></ param >
        /// < param name="matorginal"></param>
        /// <param name = "first" ></ param >
        /// < returns ></ returns >
        public List<List<int>> final(int[,] mat, int[,] matorginal, int first, Dictionary<int, List<int>> dr)
        {
            Dictionary<int, List<int>> d = new Dictionary<int, List<int>>();
            List<List<int>> lstSlo = new List<List<int>>();
            for (int j = 0; j < mat.GetLength(0); j++)
            {
                List<int> Rows = new List<int>(), Colunms = new List<int>();
                List<int> finalR = new List<int>();
                int[,] m = GetSubmatrix(mat, 0, j);//תת מטריצה נכון
                if (mat[0, j] == 0)
                {
                    if (Rec(m))
                    {


                        finalR.Add(matorginal[0, j]);
                        Rows.Add(0);
                        Colunms.Add(j);

                        for (int a = 0; a < mat.GetLength(0); a++)
                        {
                            for (int b = 0; b < mat.GetLength(0); b++)
                            {
                                if (mat[a, b] == 0)
                                {
                                    int[,] mm = GetSubmatrix(mat, a, b);
                                    if (Rec(mm) && !Colunms.Contains(b) && !Rows.Contains(a))
                                    {
                                        finalR.Add(matorginal[a, b]);
                                        Rows.Add(a);
                                        Colunms.Add(b);
                                    }
                                }
                            }
                        }
                    }
                }
                if (finalR.Count() == mat.GetLength(0))
                {
                    finalR.Add(first);
                    if (!IsFind(finalR, dr))
                        lstSlo.Add(finalR);
                }
            }
            return lstSlo;
        }
        //הכנסה למילון כל הערכים שבשורה i

        public void EnterToRowDictanery(int index, List<int> lstValues) => Drows.Add(index, lstValues);
        public void EnterToCoulnmDictanery(int index, List<int> lstVales) => Dcoulnms.Add(index, lstVales);

        //יצירת רשימה של ערכים בשורה i
        public List<int> craeteValyeListRow(int index, int tutors, int[,] mat)
        {
            List<int> lst = new List<int>();
            for (int i = 0; i < tutors; i++)
            {
                lst.Add(mat[index, i]);
            }
            return lst;
        }
        //יצירת רשימה של ערכים בשורה j
        public List<int> craeteValyeListCoulnm(int index, int apprentice, int[,] mat)
        {
            List<int> lst = new List<int>();
            for (int j = 0; j < apprentice; j++)
            {
                lst.Add(mat[j, index]);
            }
            return lst;
        }
        //סימון  השורות ב 0 ללא השיבוץ
        public void CoverageLines(int[,] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                int l = ZeroInLine(mat, i, mat.GetLength(1));
                if (l > 1 && !isrowInD(i))
                {
                    Lines++;
                    //סימון השורה כלא משובצת
                    lstNonOptimalRow = craeteValyeListRow(i, mat.GetLength(1), mat);
                    EnterToRowDictanery(i, lstNonOptimalRow);


                }
            }
        }
        public bool isCoulnmInD(int c)
        {
            foreach (KeyValuePair<int, List<int>> kvp in Dcoulnms)
            {
                if (kvp.Key == c)
                    return true;
            }
            return false;
        }
        public bool isrowInD(int r)
        {
            foreach (KeyValuePair<int, List<int>> kvp in Drows)
            {
                if (kvp.Key == r)
                    return true;
            }
            return false;
        }
        //סימון "באיקסים" את ה0 שלא יכולים להיות כמוקצים
        public void MarkedX(int[,] mat, int row, int cell)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                if (i == row)
                {
                    for (int r = 0; r < mat.GetLength(0); r++)
                    {
                        Point p = new Point(i, r);
                        if (mat[i, r] == 0 && !d.Values.Contains(p) && !dNon.Values.Contains(p))
                        {
                            //סימון "x" על 0 זה
                            Point p1 = new Point(i, r);
                            dNon.Add(key++, p1);
                        }
                    }
                }
                if (i == cell)
                {
                    for (int c = 0; c < mat.GetLength(0); c++)
                    {
                        Point p = new Point(c, i);
                        if (mat[c, i] == 0 && !d.Values.Contains(p))
                        {
                            //סימון "x" על 0 זה
                            Point p1 = new Point(c, i);
                            dNon.Add(key++, p1);
                        }
                    }
                }
            }
        }
        //בודק האם יש 0 במטריצה שלא נמצא באף מילון
        public bool iSZero(int[,] mat)
        {

            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(0); j++)
                {
                    if (mat[i, j] == 0)
                    {
                        Point point = new Point(i, j);
                        if (!d.Values.Contains(point) && !dNon.Values.Contains(point))
                            return false;
                    }
                }
            }
            return true;
        }
        public int zeroInlineExtrx(int[,] mat, int row, int tutors)
        {

            int count = 0;

            for (int i = 0; i < tutors; i++)
            {
                Point p = new Point(row, i);
                // if (mat[row, i] == 0 && dNon.Values.Contains(p) && !d.Values.Contains(p))
                if (mat[row, i] == 0 && !dNon.Values.Contains(p) && !d.Values.Contains(p))
                    count++;
            }
            return count;

        }
        public int zeroInCellExtrx(int[,] mat, int cell, int apprentice)
        {

            int count = 0;

            for (int i = 0; i < apprentice; i++)
            {
                Point p = new Point(i, cell);
                if (mat[i, cell] == 0 && !dNon.Values.Contains(p) && !d.Values.Contains(p))
                    count++;
            }
            return count;

        }
        //שלב 1
        public void step1(int[,] mat)
        {


            bool flag = false;
            while (!iSZero(mat))
            {
                flag = false;
                //אפסים בשורות
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    int zeroinrow = ZeroInLine(mat, i, mat.GetLength(0));
                    int remainderInRow = zeroInlineExtrx(mat, i, mat.GetLength(0));
                    for (int j = 0; j < mat.GetLength(1); j++)
                    {
                        Point p = new Point(i, j);
                        if (mat[i, j] == 0 && !d.Values.Contains(p) && !dNon.Values.Contains(p))
                        {
                            if (zeroinrow == 1 || (remainderInRow == 1 && zeroinrow > 1))
                            {
                                d.Add(key++, p);
                                MarkedX(mat, i, j);
                                linesoptimal[i] = 1;
                                flag = true;
                                break;
                            }
                        }
                    }
                }

                //אפסים בעמודות
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    for (int j = 0; j < mat.GetLength(1); j++)
                    {
                        int zeroincell = ZeroInColum(mat, j, mat.GetLength(0));
                        int remainderIncell = zeroInCellExtrx(mat, j, mat.GetLength(0));
                        Point p = new Point(i, j);
                        if (mat[i, j] == 0 && !d.Values.Contains(p) && !dNon.Values.Contains(p))
                        {
                            if (zeroincell == 1 || (remainderIncell == 1 && zeroincell > 1))
                            {
                                d.Add(key++, p);
                                MarkedX(mat, i, j);
                                linesoptimal[i] = 1;
                                flag = true;
                                break;

                            }
                        }
                    }
                }
                if (flag == false)
                {
                    for (int i = 0; i < mat.GetLength(0); i++)
                    {
                        for (int j = 0; j < mat.GetLength(1); j++)
                        {
                            Point p = new Point(i, j);
                            if (mat[i, j] == 0 && !d.Values.Contains(p) && !dNon.Values.Contains(p) && flag == false)
                            {
                                Point p1 = new Point(i, j);
                                linesoptimal[i] = 1;
                                d.Add(key++, p1);
                                MarkedX(mat, i, j);
                                flag = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public bool Step2(int[,] mat)
        {
            bool flag = false;
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                if (linesoptimal[i] == 0)//אם זה שורה לא מסומנת
                {
                    if (!optimalR.Contains(i))
                        optimalR.Add(i);//סימון בv  את השורה 
                    for (int a = 0; a < mat.GetLength(0); a++)
                    {
                        Point p = new Point(i, a);
                        if (mat[i, a] == 0 && dNon.Values.Contains(p) && cells[a] == 0)//אם נמצא בה 0 כלשהוא
                        {
                            cells[a] = 1;//סימון העמודה
                            flag = true;//אם יש שינוי
                        }
                    }
                }
            }
            return flag;
        }
        public bool Step3(int[,] mat)
        {
            bool flag = false;
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                if (cells[i] == 1)//אם   העמודה מסומנת וגם לשורה יש הקצאה 
                {
                    for (int x = 0; x < mat.GetLength(0); x++)
                    {
                        Point p = new Point(x, i);
                        if (d.Values.Contains(p))//סימון  מוקצה
                        {
                            if (!optimalR.Contains(p.X))
                                optimalR.Add(p.X);
                            linesoNotptimal[x] = 1;
                            linesoptimal[x] = 0;
                            // linesoptimal[x] = 0;
                            flag = true;//אם יש שינוי
                        }
                    }
                }
                linesoNotptimal[i] = 1;//סימון כשורה לא מסומנת
            }
            return flag;
        }
        public void Step5(int[,] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                if (!optimalR.Contains(i) && !isrowInD(i))
                {
                    lstNonOptimalRow = craeteValyeListRow(i, mat.GetLength(1), mat);
                    EnterToRowDictanery(i, lstNonOptimalRow);
                    Lines++;
                }
                for (int j = 0; j < mat.GetLength(0); j++)
                {
                    if (cells[j] == 1 && !isCoulnmInD(j))//אם העמודה מוקצה
                    {
                        lstNonOptimalCoulnm = craeteValyeListCoulnm(j, mat.GetLength(0), mat);
                        Dcoulnms.Add(j, lstNonOptimalCoulnm);
                        Lines++;
                    }

                }
            }
        }

        //   line מס אפסים בשורה  
        public int ZeroInLine(int[,] mat, int line, int tutors)
        {
            int count = 0;

            for (int i = 0; i < tutors; i++)
            {
                if (mat[line, i] == 0)
                    count++;
            }
            return count;
        }
        //   colum  מס אפסים בעמודה  
        public int ZeroInColum(int[,] mat, int colum, int apprentices)
        {
            int count = 0;
            for (int i = 0; i < apprentices; i++)
            {
                if (mat[i, colum] == 0)
                    count++;
            }
            return count;
        }

        //בדיקה האם קיים שיבוץ אופטימלי
        public bool IsOptimal(int[,] mat, int apprentices, int tutors)
        {
            for (int i = 0; i < apprentices; i++)
            {
                int l = ZeroInLine(mat, i, mat.GetLength(0));
                int c = ZeroInColum(mat, i, mat.GetLength(0));
                if (l > 1 || c > 1)
                    return false;
                continue;
            }
            return true;
        }
        //ערך מינימלי בשורה - שלב 1
        public int GetMinValueInRow(int[,] mat, int coulm, int line)
        {
            int min = 10000000;
            for (int j = 0; j < coulm; j++)
            {
                if (mat[line, j] < min)
                    min = (int)mat[line, j];
            }
            return min;
        }
        //ערך מינימלי בעמודה - שלב 2
        public int GetMinValueInColumn(int[,] mat, int row, int column)
        {
            int min = 1000000;

            for (int i = 0; i < row; i++)
            {
                if (mat[i, column] <= min)
                    min = (int)mat[i, column];
            }
            return min;
        }
        //שורות בדיקה האם מס מסוים נמצא במילון
        public bool IsInDRow(int num, int row)
        {
            foreach (KeyValuePair<int, List<int>> kvp in Drows)
            {
                if (kvp.Key != row)
                {
                    continue;
                }

                foreach (var _ in kvp.Value.Where(item => item == num).Select(item => new { }))
                {
                    return true;
                }
            }
            return false;
        }
        //בדיקה האם ערך מסוים נמצא במילון עמודות
        public bool IsInDCouknm(int num, int coulnm)
        {
            foreach (KeyValuePair<int, List<int>> kvp in Dcoulnms)
            {
                if (kvp.Key != coulnm)
                {
                    continue;
                }

                foreach (var _ in kvp.Value.Where(item => item == num).Select(item => new { }))
                {
                    return true;
                }
            }
            return false;
        }
        //הפחתת הערך המינימלי שלא מכוסה
        public int GetMinValueInMat(int[,] mat)
        {
            int min = 10000;
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    //אם הערך הנוכחי לא 0 וגם קטן מהמינימום וגם 
                    if (mat[i, j] != 0 && mat[i, j] < min && !isCoulnmInD(j) && !isrowInD(i))
                        min = (int)mat[i, j];
                }
            }
            return min;
        }
        /// <summary>
        /// Gets a matrix and creates the Hungarian
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public void Hungary(int[,] mat)
        {
            int min;
            Lines = 0;
            int minInMat = 10000;
            int row = mat.GetLength(0);
            int coulm = mat.GetLength(1);
            int[,] currentMat = mat;
            //שלב 1
            for (int g = 0; g < row; g++)
            {
                //i ערך מינימילי בשורה 
                min = GetMinValueInRow(mat, coulm, g);
                //  i הפחתת ערך מינימלי ב שורה           
                for (int x = 0; x < coulm; x++)
                    currentMat[g, x] -= min;
            }
            //שלב 2
            for (int o = 0; o < coulm; o++)
            {
                // j ערך מינימילי בעמודה
                min = GetMinValueInColumn(mat, row, o);
                //  j הפחתת ערך מינימלי בעמודה  
                for (int c = 0; c < row; c++)
                {
                    currentMat[c, o] -= min;
                }
            }
            while (Lines < currentMat.GetLength(0))
            {
                Lines = 0;

                bool isChangeStep2 = true, isChangeStep3 = true;
                Drows = new Dictionary<int, List<int>>();
                Dcoulnms = new Dictionary<int, List<int>>();
                d = new Dictionary<int, Point>();
                dNon = new Dictionary<int, Point>();
                linesoNotptimal = new int[currentMat.GetLength(0)];
                linesoptimal = new int[currentMat.GetLength(0)];
                cells = new int[currentMat.GetLength(0)];
                optimalR = new List<int>();
                step1(currentMat);
                isChangeStep2 = Step2(currentMat);
                isChangeStep3 = Step3(currentMat);
                while (isChangeStep2 && isChangeStep3)
                {
                    isChangeStep2 = Step2(currentMat);
                    isChangeStep3 = Step3(currentMat);
                }
                Step5(currentMat);
                if (Lines < currentMat.GetLength(0) && Drows.Count < currentMat.GetLength(0))
                //שלב 5
                {

                    minInMat = GetMinValueInMat(currentMat);
                    Addtodouble(minInMat);
                    UnCoveredLine(minInMat);
                }
                if (Drows.Count + Dcoulnms.Count == currentMat.GetLength(0) && Drows.Count > 0 && Dcoulnms.Count > 0)
                {
                    isChangeStep2 = Step2(currentMat);
                    isChangeStep3 = Step3(currentMat);
                    while (isChangeStep2 && isChangeStep3)
                    {
                        isChangeStep2 = Step2(currentMat);
                        isChangeStep3 = Step3(currentMat);
                    }
                }

            }
        }
    }
}


