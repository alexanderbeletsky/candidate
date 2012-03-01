using System;
using System.Collections.Generic;
using System.IO;

namespace Bounce.Framework {
    public class Copy : Task {
        public static readonly List<string> SvnDirectories = new List<string> { @"**\_svn\", @"**\.svn\" };

        private readonly IFileSystemCopier FileSystemCopier;

        public Copy() : this(new FileSystemCopier()) {}

        public Copy(IFileSystemCopier fileSystemCopier) {
            FileSystemCopier = fileSystemCopier;
            DeleteToDirectory = true;
        }

        /// <summary>
        /// A file or directory to copy from
        /// </summary>
        [Dependency, CleanAfterBuild]
        public Task<string> FromPath { get; set; }

        [Dependency]
        private Task<string> _toPath;

        /// <summary>
        /// A file or directory to copy to
        /// </summary>
        public Task<string> ToPath
        {
            get { return this.WhenBuilt(() => _toPath.Value); }
            set { _toPath = value; }
        }

        /// <summary>
        /// Glob patterns of files and directories not to copy
        /// </summary>
        [Dependency]
        public Task<IEnumerable<string>> Excludes;
        /// <summary>
        /// Glob patterns of files and directories to copy, overriding Excludes
        /// </summary>
        [Dependency]
        public Task<IEnumerable<string>> Includes;

        [Dependency]
        public Task<bool> DeleteToDirectory;

        public override void Build(IBounce bounce) {
            var fromPath = FromPath.Value;
            var toPath = _toPath.Value;

            bounce.Log.Debug("copying from: `{0}', to: `{1}'", fromPath, toPath);

            FileSystemCopier.Copy(fromPath, toPath, GetValueOf(Excludes), GetValueOf(Includes), DeleteToDirectory.Value);
        }

        private IEnumerable<string> GetValueOf(Task<IEnumerable<string>> paths) {
            return paths != null ? paths.Value : null;
        }

        public override void Clean() {
            FileSystemCopier.Delete(_toPath.Value);
        }
    }
}