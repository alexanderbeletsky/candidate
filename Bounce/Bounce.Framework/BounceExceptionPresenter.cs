﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Bounce.Framework {
    class BounceExceptionPresenter : IBounceExceptionPresenter {
        private TextWriter Stderr;

        public BounceExceptionPresenter(TextWriter stderr) {
            Stderr = stderr;
        }

        public void CommandLineParameters(IEnumerable<ParameterError> errors) {
            
        }

        public void CommandError(string output) {
        }

        public void TaskException(ITask task, string message) {
        }

        public void GeneralException(BounceException bounceException) {
            Stderr.WriteLine(bounceException);
        }
    }
}