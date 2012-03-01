using System;
using System.IO;

namespace Bounce.Framework {
    public class DirectoryFiles {
        public Task<string> Root { get; set; }

        public DirectoryFiles(Task<string> root) {
            Root = root;
        }

        public Task<string> this[Task<string> filename] {
            get {
                return new All(Root, filename).WhenBuilt(() => Path.Combine(Root.Value, filename.Value));
            }
        }
    }
}