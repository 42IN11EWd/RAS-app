using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.MVVM.CommandBehaviour
{
    /// <summary>
    /// Defines the interface for a strategy of execution for the CommandBehaviourBinding
    /// </summary>
    public interface IExecutionStrategy
    {
        /// <summary>
        /// Gets or sets the behaviour that we execute this strategy
        /// </summary>
        CommandBehaviourBinding behaviour { get; set; }

        /// <summary>
        /// Executes according to the strategy type
        /// </summary>
        /// <param name="parameter">The parameter to be used in the execution</param>
        void Execute(object parameter);
    }

    /// <summary>
    /// Executes a command 
    /// </summary>
    public class CommandExecutionStrategy : IExecutionStrategy
    {
        #region IExecutionStrategy Members
        /// <summary>
        /// Gets or sets the behaviour that we execute this strategy
        /// </summary>
        public CommandBehaviourBinding behaviour { get; set; }

        /// <summary>
        /// Executes the Command that is stored in the CommandProperty of the CommandExecution
        /// </summary>
        /// <param name="parameter">The parameter for the command</param>
        public void Execute(object parameter)
        {
            if (behaviour == null)
                throw new InvalidOperationException("Behaviour property cannot be null when executing a strategy");

            if (behaviour.Command.CanExecute(behaviour.CommandParameter))
                behaviour.Command.Execute(behaviour.CommandParameter);
        }

        #endregion
    }

    /// <summary>
    /// executes a delegate
    /// </summary>
    public class ActionExecutionStrategy : IExecutionStrategy
    {

        #region IExecutionStrategy Members

        /// <summary>
        /// Gets or sets the behaviour that we execute this strategy
        /// </summary>
        public CommandBehaviourBinding behaviour { get; set; }

        /// <summary>
        /// Executes an Action delegate
        /// </summary>
        /// <param name="parameter">The parameter to pass to the Action</param>
        public void Execute(object parameter)
        {
            behaviour.Action(parameter);
        }

        #endregion
    }
}
