
    class SimpleStack<T>
    {

        private List<T> data = new List<T>();

        private object lockObject = new object();

        public T Top { 
            get {

                lock (lockObject)
                {
                    int idx = this.data.Count - 1;
                    if (idx == -1)
                    {
                        throw new StackEmptyException();
                    }                  
                    return data[idx];
                }
            } 
        }


        public bool IsEmpty
        {
            get
            {
                return this.data.Count == 0;
            }
        }


        public void Push(T val) {

            lock (lockObject)
            {
                this.data.Add(val);
            }
        
        }


        public bool TryPop(out T val)
        {
            lock (lockObject)
            {
                int idx = this.data.Count - 1;
                if (idx == -1)
                {
                    val = default;
                    return false;
                }
                val = this.data[idx];
                this.data.RemoveAt(idx);
                return true;
            }
        }


        public class StackEmptyException : Exception
        {

        }
    }
