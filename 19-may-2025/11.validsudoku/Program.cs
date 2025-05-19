class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter a 9x9 Sudoku board");
        int[][] board = new int[9][];
        for (int i = 0; i < 9; i++)
        {
            string? line = Console.ReadLine();

            if (line != null)
            {
                var digits = new List<int>();

                foreach (char c in line)
                {
                    if (char.IsWhiteSpace(c)) 
                    continue; 

                    if (int.TryParse(c.ToString(), out int value))
                    {
                        digits.Add(value);
                    }
                    else
                    {
                        Console.WriteLine($"Invalid character '{c}' — skipping.");
                    }
                }

                if (digits.Count == 9)
                {
                    board[i] = digits.ToArray();
                }
                else
                {
                    Console.WriteLine($"Row {i + 1} must have exactly 9 digits. Found {digits.Count}. Please re-enter.");
                    i--;
                }
            }
        }
        //   int[][] board = new int[][]
        //     {
        //         new int[] {5, 3, 4, 6, 7, 8, 9, 1, 2},
        //         new int[] {6, 7, 2, 1, 9, 5, 3, 4, 8},
        //         new int[] {1, 9, 8, 3, 4, 2, 5, 6, 7},
        //         new int[] {8, 5, 9, 7, 6, 1, 4, 2, 3},
        //         new int[] {4, 2, 6, 8, 5, 3, 7, 9, 1},
        //         new int[] {7, 1, 3, 9, 2, 4, 8, 5, 6},
        //         new int[] {9, 6, 1, 5, 3, 7, 2, 8, 4},
        //         new int[] {2, 8, 7, 4, 1, 9, 6, 3, 5},
        //         new int[] {3, 4, 5, 2, 8, 6, 1, 7, 9}
        //     };
        bool isValid = IsValidSudoku(board);
        Console.WriteLine(isValid ? "Valid Sudoku" : "Invalid Sudoku");
    }
    static bool IsValidSudoku(int[][] board)
    {
        HashSet<int>[] rows = new HashSet<int>[9];
        HashSet<int>[] cols = new HashSet<int>[9];
        HashSet<int>[] boxes = new HashSet<int>[9];
        for (int i = 0; i < 9; i++)
        {
            rows[i] = new HashSet<int>();
            cols[i] = new HashSet<int>();
             boxes[i] = new HashSet<int>();
        }

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (rows[r].Contains(board[r][c]))
                {
                    Console.WriteLine($"Invalid Sudoku, row {r} has duplicate value {board[r][c]}");
                    return false;
                }
                else
                {
                    rows[r].Add(board[r][c]);
                }
                if (cols[c].Contains(board[r][c]))
                {
                    Console.WriteLine($"Invalid Sudoku, column {c} has duplicate value {board[r][c]}");
                    return false;
                }
                else
                {

                    cols[c].Add(board[r][c]);
                }
                int boxIndex = (r / 3) * 3 + (c / 3);
            if (boxes[boxIndex].Contains(board[r][c]))
            {
                Console.WriteLine($"Invalid Sudoku, 3x3 box {boxIndex} has duplicate value {board[r][c]}");
                return false;
            }
            boxes[boxIndex].Add(board[r][c]);

            }
        }
        return true;
    }
}



