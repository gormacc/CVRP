using System.Collections.Generic;
using CVRP.Model;

namespace CVRP
{
    public static class TestDataSets
    {
        public static TestData GetFirstTestDataSet()
        {
            var vertexes = new List<TestDataVertex>
            {
                new TestDataVertex(1, 145, 215, 0),
                new TestDataVertex(2, 151, 264, 1100),
                new TestDataVertex(3, 159, 261, 700),
                new TestDataVertex(4, 130, 254, 800),
                new TestDataVertex(5, 128, 252, 1400),
                new TestDataVertex(6, 163, 247, 2100),
                new TestDataVertex(7, 146, 246, 400),
                new TestDataVertex(8, 161, 242, 800),
                new TestDataVertex(9, 142, 239, 100),
                new TestDataVertex(10, 163, 236, 500),
                new TestDataVertex(11, 148, 232, 600),
                new TestDataVertex(12, 128, 231, 1200),
                new TestDataVertex(13, 156, 217, 1300),
                new TestDataVertex(14, 129, 214, 1300),
                new TestDataVertex(15, 146, 208, 300),
                new TestDataVertex(16, 164, 208, 900),
                new TestDataVertex(17, 141, 206, 2100),
                new TestDataVertex(18, 147, 193, 1000),
                new TestDataVertex(19, 164, 193, 900),
                new TestDataVertex(20, 129, 189, 2500),
                new TestDataVertex(21, 155, 185, 1800),
                new TestDataVertex(22, 139, 182, 700)
            };

            return new TestData(6000, 4, 375, vertexes);
        }

        public static TestData GetSecondTestDataSet()
        {
            var vertexes = new List<TestDataVertex>
            {
                new TestDataVertex(1, 292, 495, 0),
                new TestDataVertex(2, 298, 427, 700),
                new TestDataVertex(3, 309, 445, 400),
                new TestDataVertex(4, 307, 464, 400),
                new TestDataVertex(5, 336, 475, 1200),
                new TestDataVertex(6, 320, 439, 40),
                new TestDataVertex(7, 321, 437, 80),
                new TestDataVertex(8, 322, 437, 2000),
                new TestDataVertex(9, 323, 433, 900),
                new TestDataVertex(10, 324, 433, 600),
                new TestDataVertex(11, 323, 429, 750),
                new TestDataVertex(12, 314, 435, 1500),
                new TestDataVertex(13, 311, 442, 150),
                new TestDataVertex(14, 304, 427, 250),
                new TestDataVertex(15, 293, 421, 1600),
                new TestDataVertex(16, 296, 418, 450),
                new TestDataVertex(17, 261, 384, 700),
                new TestDataVertex(18, 297, 410, 550),
                new TestDataVertex(19, 315, 407, 650),
                new TestDataVertex(20, 314, 406, 200),
                new TestDataVertex(21, 321, 391, 400),
                new TestDataVertex(22, 321, 398, 300),
                new TestDataVertex(23, 314, 394, 1300),
                new TestDataVertex(24, 313, 378, 700),
                new TestDataVertex(25, 304, 382, 750),
                new TestDataVertex(26, 295, 402, 1400),
                new TestDataVertex(27, 283, 406, 4000),
                new TestDataVertex(28, 279, 399, 600),
                new TestDataVertex(29, 271, 401, 1000),
                new TestDataVertex(30, 264, 414, 500),
                new TestDataVertex(31, 277, 439, 2500),
                new TestDataVertex(32, 290, 434, 1700),
                new TestDataVertex(33, 319, 433, 1100),
            };

            return new TestData(8000, 4, 835, vertexes);
        }

        public static TestData GetThirdTestDataSet()
        {
            var vertexes = new List<TestDataVertex>
            {
                new TestDataVertex(1, 30, 40, 0),
                new TestDataVertex(2, 37, 52, 7),
                new TestDataVertex(3, 49, 49, 30),
                new TestDataVertex(4, 52, 64, 16),
                new TestDataVertex(5, 20, 26, 9),
                new TestDataVertex(6, 40, 30, 21),
                new TestDataVertex(7, 21, 47, 15),
                new TestDataVertex(8, 17, 63, 19),
                new TestDataVertex(9, 31, 62, 23),
                new TestDataVertex(10, 52, 33, 11),
                new TestDataVertex(11, 51, 21, 5),
                new TestDataVertex(12, 42, 41, 19),
                new TestDataVertex(13, 31, 32, 29),
                new TestDataVertex(14, 5, 25, 23),
                new TestDataVertex(15, 12, 42, 21),
                new TestDataVertex(16, 36, 16, 10),
                new TestDataVertex(17, 52, 41, 15),
                new TestDataVertex(18, 27, 23, 3),
                new TestDataVertex(19, 17, 33, 41),
                new TestDataVertex(20, 13, 13, 9),
                new TestDataVertex(21, 57, 58, 28),
                new TestDataVertex(22, 62, 42, 8),
                new TestDataVertex(23, 42, 57, 8),
                new TestDataVertex(24, 16, 57, 16),
                new TestDataVertex(25, 8, 52, 10),
                new TestDataVertex(26, 7, 38, 28),
                new TestDataVertex(27, 27, 68, 7),
                new TestDataVertex(28, 30, 48, 15),
                new TestDataVertex(29, 43, 67, 14),
                new TestDataVertex(30, 58, 48, 6),
                new TestDataVertex(31, 58, 27, 19),
                new TestDataVertex(32, 37, 69, 11),
                new TestDataVertex(33, 38, 46, 12),
                new TestDataVertex(34, 46, 10, 23),
                new TestDataVertex(35, 61, 33, 26),
                new TestDataVertex(36, 62, 63, 17),
                new TestDataVertex(37, 63, 69, 6),
                new TestDataVertex(38, 32, 22, 9),
                new TestDataVertex(39, 45, 35, 15),
                new TestDataVertex(40, 59, 15, 14),
                new TestDataVertex(41, 5, 6, 7),
                new TestDataVertex(42, 10, 17, 27),
                new TestDataVertex(43, 21, 10, 13),
                new TestDataVertex(44, 5, 64, 11),
                new TestDataVertex(45, 30, 15, 16),
                new TestDataVertex(46, 39, 10, 10),
                new TestDataVertex(47, 32, 39, 5),
                new TestDataVertex(48, 25, 32, 25),
                new TestDataVertex(49, 25, 55, 17),
                new TestDataVertex(50, 48, 28, 18),
                new TestDataVertex(51, 56, 37, 10)
            };

            return new TestData(160, 6, 521, vertexes);
        }
    }
}
