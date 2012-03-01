﻿namespace Bounce.Framework {
    internal class DependencyBuildFailureException : TaskException {
        public DependencyBuildFailureException(ITask task, string message) : base(task, message) {
        }
    }
}