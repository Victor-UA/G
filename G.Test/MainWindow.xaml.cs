using G.Base;
using G.Service;
using G.Test.Models;
using RP.Math;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Diagnostics;

namespace G.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isRun;
        private Player _player = null;
        private IDictionary<int, IEntityModel> _entitiesModels;
        private object _lock = new object();

        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void b_Start_Click(object sender, RoutedEventArgs e)
        {
            if (!isRun)
            {
                isRun = true;

                StartService();                

                _entitiesModels = new Dictionary<int, IEntityModel>();

                Random rnd = new Random();
                _player = MainService.Instance.AddPlayer();

                DrawGamePlay();                

                ConnectNewPlayers(1);
                MoveAIPlayers();
            }
            else
            {
                isRun = false;
                MainService.Instance.ManualStop();
                foreach (var item in _entitiesModels.Values)
                {
                    item.Dispose();
                }              
            }
        }

        private void ConnectNewPlayers(int count)
        {
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    MainService.Instance.AddPlayer();
                }
            });
        }
        private void KillPlayers(int count)
        {
            Task.Factory.StartNew(() =>
            {
                if (_entitiesModels?.Count > 0)
                {
                    Random rnd = new Random();
                    int[] keys = _entitiesModels.Keys.ToArray();
                    for (int i = 0; i < count; i++)
                    {
                        int index = rnd.Next(keys.Length);
                        int key = keys[index];

                        if (_player.ID != key)
                        {
                            MainService.Instance.KillPlayer(key);
                        }
                    }
                }
            });
        }

        private void MoveAIPlayers()
        {
            Task.Factory.StartNew(() =>
            {
                Random rnd = new Random();
                while (true)
                {
                    foreach (var entity in MainService.Instance.GetEntities().Values)
                    {
                        if (entity is Player && entity.ID != _player?.ID)
                        {
                            var player = entity as Player;
                            player.MoveState.Forward = rnd.NextDouble() > 0.3;
                            double tmp = rnd.NextDouble();
                            player.MoveState.RotateLeft = tmp > 0.6;
                            player.MoveState.RotateRight = tmp < 0.4;
                            MainService.Instance.SetPlayerStates(player.ID, player.MoveState);
                        }
                    }
                    Thread.Sleep(250);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void StartService()
        {
            if (!MainService.Instance.IsRun)
            {
                MainService.Instance.ManualStart(null);
            }
        }

        private void DrawGamePlay()
        {            
            int framesCount = 0;
            Task.Factory.StartNew(() =>
            {
                while (isRun)
                {
                    framesCount = 0;
                    var fpsWatch = new Stopwatch();
                    fpsWatch.Start();
                    Thread.Sleep(500);

                    Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            tb_FPS.Text = (framesCount / fpsWatch.Elapsed.TotalSeconds).ToString("0") + " fps";
                            tb_PlayersCount.Text = MainService.Instance.GetEntities().Count.ToString() + " players";
                            tb_PlayerX.Text = _player.Position.Location.X.ToString("0");
                            tb_PlayerY.Text = _player.Position.Location.Y.ToString("0");
                            tb_PlayerVelocityAbs.Text = (_player.Velocity.Magnitude).ToString("0");
                            tb_PlayerMaxIterrationTime.Text = Entity.MaxIterrationTime.ToString();
                        }
                        catch (Exception) { }
                    });
                }
            }, TaskCreationOptions.LongRunning);

            int SleepTime = 10;
            var IterationTime = new Stopwatch();

            Task.Factory.StartNew(() =>
            {
                while (isRun)
                {
                    IterationTime.Restart();
                    Dispatcher.Invoke(() =>
                    {
                        IDictionary<int, Entity> Entities = MainService.Instance.GetEntities();
                        foreach (var item in Entities.Values)
                        {
                            IEntityModel model;
                            if (_entitiesModels.ContainsKey(item.ID))
                            {
                                model = _entitiesModels[item.ID];
                                model.Data = item;
                            }
                            else
                            {
                                if (item is Player)
                                {
                                    model = new PlayerModel(item as Player);
                                    _entitiesModels.Add(item.ID, model);
                                }
                                else
                                {
                                    throw (new NotSupportedException());
                                }
                            }

                            model.Draw(c_Main1);
                            if (!isRun)
                            {
                                return;
                            }
                        }

                        var entityModelsToDispose = _entitiesModels.Values.Where(item => !Entities.ContainsKey(item.Data.ID)).ToArray();
                        foreach (var entityModel in entityModelsToDispose)
                        {
                            entityModel.Dispose();
                            _entitiesModels.Remove(entityModel.Data.ID);
                        }
                    });                    
                    framesCount++;
                    IterationTime.Stop();
                    SleepTime += (int)((IterationTime.ElapsedMilliseconds * 10) - SleepTime) / 3;
                    SleepTime = SleepTime < 10 ? 10 : SleepTime > 25 ? 25 : SleepTime;
                    Thread.Sleep(SleepTime);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (isRun)
            {
                switch (e.Key)
                {
                    case System.Windows.Input.Key.Up:
                        _player.MoveState.Forward = true;
                        break;
                    case System.Windows.Input.Key.Down:
                        _player.MoveState.Backward = true;
                        break;
                    case System.Windows.Input.Key.Left:
                        _player.MoveState.RotateLeft = true;
                        break;
                    case System.Windows.Input.Key.Right:
                        _player.MoveState.RotateRight = true;
                        break;                    
                }
                e.Handled = true;
                MainService.Instance.SetPlayerStates(_player.ID, _player.MoveState);
            }
        }

        private void Grid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (isRun)
            {
                switch (e.Key)
                {
                    case System.Windows.Input.Key.Up:
                        _player.MoveState.Forward = false;
                        break;
                    case System.Windows.Input.Key.Down:
                        _player.MoveState.Backward = false;
                        break;
                    case System.Windows.Input.Key.Left:
                        _player.MoveState.RotateLeft = false;
                        break;
                    case System.Windows.Input.Key.Right:
                        _player.MoveState.RotateRight = false;
                        break;
                }
                MainService.Instance.SetPlayerStates(_player.ID, _player.MoveState);
            }
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isRun = false;
            Thread.Sleep(100);
        }

        private void b_RemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            KillPlayers(50);
        }

        private void b_ConnectPlayer_Click(object sender, RoutedEventArgs e)
        {
            ConnectNewPlayers(50);
        }
    }
}
