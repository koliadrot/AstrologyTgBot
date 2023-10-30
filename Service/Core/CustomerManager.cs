namespace Service.Core
{
    using Data.Core;
    using Service.Abstract;
    using System;

    public class CustomerManager : ICustomerManager, IDisposable
    {
        private readonly ApplicationDbContext _bonusDbContext;

        public CustomerManager()
        {
            _bonusDbContext = new ApplicationDbContext();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bonusDbContext.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

        public bool ExistTelegram(long telegramId)
        {
            //bool result = false;
            //foreach (Client client in _bonusDbContext.Clients.Where(client => client.IdTlgrm == telegramId.ToString()))
            //{
            //    TransitionParam transitionParam = _settingsManager.GetTransitionParamByClientAccountOfOldBonusProgram(client);
            //    if (transitionParam == null)
            //    {
            //        result = true;
            //    }
            //}

            //return result;
            return false;
        }
    }
}
