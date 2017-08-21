using Stone.DataModel.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.DataModel.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...
        private StoneEntities _context = null;
        private GenericRepository<Card> _cardRepository;
        private GenericRepository<CardBrand> _cardBrandRepository;
        private GenericRepository<CardType> _cardTypeRepository;
        private GenericRepository<Transaction> _transactionRepository;
        private GenericRepository<TransactionType> _transactionTypeRepository;
        #endregion

        public UnitOfWork()
        {
            _context = new Stone.DataModel.StoneEntities();
        }

        #region Public Repository Creation properties...

        /// <summary>  
        /// Get/Set Property for Card repository.  
        /// </summary>  
        public GenericRepository<Card> CardRepository
        {
            get
            {
                if (this._cardRepository == null)
                    this._cardRepository = new GenericRepository<Card>(_context);
                return _cardRepository;
            }
        }

        /// <summary>  
        /// Get/Set Property for CardBrand repository.  
        /// </summary>  
        public GenericRepository<CardBrand> CardBrandRepository
        {
            get
            {
                if (this._cardBrandRepository == null)
                    this._cardBrandRepository = new GenericRepository<CardBrand>(_context);
                return _cardBrandRepository;
            }
        }

        /// <summary>  
        /// Get/Set Property for TransactionType repository.  
        /// </summary>  
        public GenericRepository<Transaction> TransactionRepository
        {
            get
            {
                if (this._transactionRepository == null)
                    this._transactionRepository = new GenericRepository<Transaction>(_context);
                return _transactionRepository;
            }
        }

        /// <summary>  
        /// Get/Set Property for Transaction repository.  
        /// </summary>  
        public GenericRepository<TransactionType> TransactionTypeRepository
        {
            get
            {
                if (this._transactionTypeRepository == null)
                    this._transactionTypeRepository = new GenericRepository<TransactionType>(_context);
                return _transactionTypeRepository;
            }
        }

        /// <summary>  
        /// Get/Set Property for CardType repository.  
        /// </summary>  
        public GenericRepository<CardType> CardTypeRepository
        {
            get
            {
                if (this._cardTypeRepository == null)
                    this._cardTypeRepository = new GenericRepository<CardType>(_context);
                return _cardTypeRepository;
            }
        }

        
        #endregion

        #region Public member methods...
        /// <summary>  
        /// Save method.  
        /// </summary>  
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>  
        /// Protected Virtual Dispose method  
        /// </summary>  
        /// <param name="disposing"></param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>  
        /// Dispose method  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
