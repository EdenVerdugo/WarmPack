using System;
using System.ComponentModel;

namespace WarmPack.Threading
{
    public static class Task
    {
        private static TaskDoMonitor _doMonitor = null;
        private static Exception UnHandledException = null;

        public static void Sleep(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }

        public static void Run(Action doWork)
        {
            Run(doWork, null);
        }

        public static TaskDoMonitor RunTask(Action doWork)
        {
            return RunTask(doWork, false);
        }

        internal static TaskDoMonitor RunTask(Action doWork, bool useSplash)
        {
            UnHandledException = null;

            BackgroundWorker worker = new BackgroundWorker();

            if (useSplash)
                Splash.Show();

            worker.DoWork += new DoWorkEventHandler((o, e) =>
            {
                try
                {
                    doWork.Invoke();
                }
                catch (Exception ex)
                {
                    _doMonitor.UnHandledException = new TaskException(ex.Message, ex);
                    //throw new Exception(ex.Message);
                }
            });

            _doMonitor = new TaskDoMonitor(ref worker, useSplash);

            return _doMonitor;
        }

        public static void Run(Action doWork, Action completed = null)
        {
            UnHandledException = null;

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler((o, e) =>
            {
                try
                {
                    doWork.Invoke();
                }
                catch (Exception ex)
                {
                    UnHandledException = ex;
                }
            });

            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((o, e) =>
            {
                if (UnHandledException != null)
                    throw UnHandledException;

                if (completed != null)
                {
                    completed.Invoke();
                }
            });



            worker.RunWorkerAsync();
        }

        public class TaskDoMonitor
        {
            internal TaskException UnHandledException;
            private BackgroundWorker _worker;
            private bool _useSplash;
            private Action<TaskException> _CatchAction = null;

            public TaskDoMonitor(ref BackgroundWorker worker)
            {
                this._worker = worker;
            }



            public TaskDoMonitor(ref BackgroundWorker worker, bool useSplash)
            {
                this._worker = worker;
                this._useSplash = useSplash;
            }

            /// <summary>
            /// Ejecuta la tarea asincrona.
            /// </summary>
            /// <returns></returns>
            public TaskDoMonitor Completed()
            {
                return Completed(() =>
                {

                });
            }

            /// <summary>
            /// Ejecuta una accion despues de completar la tarea asincrona
            /// </summary>
            /// <param name="completed"></param>
            /// <returns></returns>                        
            public TaskDoMonitor Completed(Action completed)
            {
                if (_worker != null)
                {
                    _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                        (o, e) =>
                        {
                            if (_useSplash)
                                Splash.Hide();

                            if (UnHandledException != null)
                                _CatchAction(UnHandledException);


                            if (UnHandledException == null || (UnHandledException?.ContinueOnException == true))
                                completed.Invoke();

                        });

                    _worker.RunWorkerAsync();
                }

                return this;
            }

            /// <summary>
            /// Ejecuta una accion en caso de que ocurra una excepcion dentro de la tarea asincrona, usar de preferencia antes de llamar al completed.
            /// </summary>
            /// <param name="action"></param>
            /// <returns></returns>
            public TaskDoMonitor Catch(Action<TaskException> action)
            {
                _CatchAction = action;

                return this;
            }
        }
    }

    public class TaskException : Exception
    {
        public bool ContinueOnException { get; set; }

        public TaskException()
        {

        }

        public TaskException(string message) : base(message)
        {

        }

        public TaskException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
