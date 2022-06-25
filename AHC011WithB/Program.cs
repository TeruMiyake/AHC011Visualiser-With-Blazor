namespace AHC011WithB
{
    internal class Program
    {
        static void Main()
        {
            Solver sol = new Solver();
            sol.Solve();
        }
    }

    public class Solver
    {
        public void Solve()
        {
        }

        /// <summary>
        /// 盤面状態を表す
        /// 変数名[i, j] でマス目の情報を読み書きできる
        /// </summary>
        public class BoardState
        {
            public int N;
            public int T;
            public int[] Placement; // 0 ~ 15
            // 直接連結しているセルの集合 (x < y)
            //public HashSet<(int x, int y)> ConnectedCells;
            // x < y なる x, y が直接接続されているか
            public bool[,] IsConnected;
            public int BlankCellIndex;


            /// <summary>
            /// C# のインデックスプロパティを用いて state[i, j] で Placement[i*N+j] つまりマス [i, j] の情報にアクセスできるようにする
            /// また、マス目の情報に 0 が代入されたら BlankCellIndex を設定する
            /// </summary>
            public int this[int i, int j]
            {
                get
                {
                    if (0 <= i && i < N && 0 <= j && j < N)
                        return Placement[i * N + j];
                    else return -1;
                }
                set
                {
                    if (0 <= i && i < N && 0 <= j && j < N)
                    {
                        if (0 <= i && i < N && 0 <= j && j < N)
                            Placement[i * N + j] = value;
                        if (value == 0) BlankCellIndex = i * N + j;
                    }
                }
            }
            public int this[int index]
            {
                get
                {
                    return this[index / N, index % N];
                }
                set
                {
                    this[index / N, index % N] = value;
                }
            }

            // 基本、これで生成してインデックスプロパティで設定するのが分かりやすいかな？
            public BoardState(int n)
            {
                N = n;
                T = n * n * n * 2;
                Placement = new int[N * N];
                //ConnectedCells = new HashSet<(int x, int y)>();
                IsConnected = new bool[N * N, N * N];
                for (int i = 0; i < N * N; i++) for (int j = 0; j < N * N; j++)
                        IsConnected[i, j] = false;
            }
            public BoardState(int n, int[] placement)
            {
                N = n;
                T = n * n * n * 2;
                Placement = placement;
                //ConnectedCells = new HashSet<(int x, int y)>();
                for (int i = 0; i < N; i++) for (int j = 0; j < N; j++)
                        IsConnected[i, j] = false;
            }

            public BoardState DeepCopy()
            {
                BoardState newState = (BoardState)this.MemberwiseClone();

                newState.Placement = new int[N * N];
                newState.IsConnected = new bool[N * N, N * N];

                for (int i = 0; i < N * N; i++) newState.Placement[i] = Placement[i];
                for (int i = 0; i < N * N; i++) for (int j = 0; j < N * N; j++)
                        newState.IsConnected[i, j] = IsConnected[i, j];

                return newState;
            }

        }

        #region CanMoveHoge, MoveHoge
        public bool CanMoveUp(int N, BoardState state)
        {
            return state.BlankCellIndex / N != 0;
        }
        public bool CanMoveDown(int N, BoardState state)
        {
            return state.BlankCellIndex / N != N - 1;
        }
        public bool CanMoveLeft(int N, BoardState state)
        {
            return state.BlankCellIndex % N != 0;
        }
        public bool CanMoveRight(int N, BoardState state)
        {
            return state.BlankCellIndex % N != N - 1;
        }
        public void MoveUp(int N, BoardState state)
        {
            int targetCellIndex = state.BlankCellIndex;
            targetCellIndex -= N;
            // swap
            // インデックスプロパティで 0 を入れると BlankCellIndex が変わってしまうので、まず先に BlankCellIndex の場所に tmp を入れる
            int tmp = state[targetCellIndex];
            state[state.BlankCellIndex] = tmp;
            state[targetCellIndex] = 0;
        }
        public void MoveDown(int N, BoardState state)
        {
            int targetCellIndex = state.BlankCellIndex;
            targetCellIndex += N;
            // swap
            // インデックスプロパティで 0 を入れると BlankCellIndex が変わってしまうので、まず先に BlankCellIndex の場所に tmp を入れる
            int tmp = state[targetCellIndex];
            state[state.BlankCellIndex] = tmp;
            state[targetCellIndex] = 0;
        }
        public void MoveLeft(int N, BoardState state)
        {
            int targetCellIndex = state.BlankCellIndex;
            targetCellIndex--;
            // swap
            // インデックスプロパティで 0 を入れると BlankCellIndex が変わってしまうので、まず先に BlankCellIndex の場所に tmp を入れる
            int tmp = state[targetCellIndex];
            state[state.BlankCellIndex] = tmp;
            state[targetCellIndex] = 0;
        }
        public void MoveRight(int N, BoardState state)
        {
            int targetCellIndex = state.BlankCellIndex;
            targetCellIndex++;
            // swap
            // インデックスプロパティで 0 を入れると BlankCellIndex が変わってしまうので、まず先に BlankCellIndex の場所に tmp を入れる
            int tmp = state[targetCellIndex];
            state[state.BlankCellIndex] = tmp;
            state[targetCellIndex] = 0;
        }
        #endregion

        #region Connection Check
        public static bool IsConnectedVertically(int upCell, int downCell)
        {
            return HasDown(upCell) && HasUp(downCell);
        }
        public static bool IsConnectedHorizontally(int leftCell, int rightCell)
        {
            return HasRight(leftCell) && HasLeft(rightCell);
        }
        public static bool HasLeft(int cell)
        {
            return (cell & 0b_0001) > 0;
        }
        public static bool HasUp(int cell)
        {
            return (cell & 0b_0010) > 0;
        }
        public static bool HasRight(int cell)
        {
            return (cell & 0b_0100) > 0;
        }
        public static bool HasDown(int cell)
        {
            return (cell & 0b_1000) > 0;
        }
        #endregion

    }
}