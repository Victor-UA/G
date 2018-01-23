using System.ServiceProcess;
using System.Threading;
using G.Logic;
using G.Base;
using System.Collections;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G.Service
{
    public sealed partial class MainService : ServiceBase
    {
        private static readonly object s_lock = new object();
        private static MainService _instance = null;
        public static MainService Instance
        {
            get
            {
                if (_instance == null)
                {
                    Monitor.Enter(s_lock);
                    MainService temp = new MainService();
                    Interlocked.Exchange(ref _instance, temp);
                    Monitor.Exit(s_lock);
                }
                return _instance;
            }
        }


        private MainService()
        {
            InitializeComponent();            
        }
        private BaseLogic Logic
        {
            get
            {
                if (_logic == null)
                {
                    Monitor.Enter(s_lock);
                    BaseLogic tmp = new BaseLogic();
                    Interlocked.Exchange(ref _logic, tmp);
                    Monitor.Exit(s_lock);
                }
                return _logic;
            }            
        }
        private BaseLogic _logic = null;

        private string[] _args;
        public bool IsRun { get; private set; }
        public bool IsPaused { get; private set; }
        private bool _commandIsManual;



        protected override void OnStart(string[] args)
        {
            if (!IsRun)
            {
                base.OnStart(args);
                IsRun = true;
                IsPaused = false;
                _args = args;
                _commandIsManual = false;
                _logic = new BaseLogic();
            }                        
        }


        protected override void OnStop()
        {
            base.Stop();
            IsRun = false;
            Logic.Dispose();
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        protected override void OnPause()
        {
            base.OnPause();
            IsPaused = true;
        }
        protected override void OnContinue()
        {
            base.OnContinue();
            IsPaused = false;
        }

        public void ManualReStart()
        {            
            _commandIsManual = true;
            OnStop();
            _commandIsManual = false;
            OnStart(_args);
        }
        public void ManualStart(string[] Args)
        {
            OnStart(Args);
        }
        public void ManualStop()
        {
            IsRun = false;
            Logic.Dispose();
            //OnStop();
        }


        public Player AddPlayer()
        {
            return Logic.AddPlayer();
        }
        public void KillPlayer(int id)
        {
            Logic.KillPlayer(id);
        }
        public void SetPlayerStates(int id, MoveStates moveState)
        {
            Logic.SetPlayerStates(id, moveState);
        }

        public IDictionary<int, Entity> GetEntities()
        {
            return Logic.GetEntities();
        }
    }
}
