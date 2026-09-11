namespace API.Service
{
    public class CorsCacheService
    {
        private readonly HashSet<string> _allowedOrigins = new();
        private readonly object _lock = new();

        public void LoadOrigins(IEnumerable<string> origins)
        {
            lock (_lock)
            {
                _allowedOrigins.Clear();
                foreach (var origin in origins)
                    _allowedOrigins.Add(origin);
            }
        }

        public void AddOrigin(string origin)
        {
            lock (_lock)
            {
                _allowedOrigins.Add(origin);
            }
        }
        public void RemoveOrigin(string origin)
        {
            lock (_allowedOrigins)
            {
                _allowedOrigins.Remove(origin);
            }
        }

        public bool IsAllowed(string origin)
        {
            lock (_lock)
            {
                return _allowedOrigins.Contains(origin);
            }
        }
    }

}