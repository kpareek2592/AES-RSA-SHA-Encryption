using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace FileEncryptionConsoleApp.SupportService
{
    public class FileExtension : IEquatable<FileExtension>
    {
        private static readonly Regex ValidFileExtensionPattern = new Regex(@"^\.[a-z0-9]{2,16}$");

        public static readonly FileExtension Wav = new FileExtension(".wav");

        private static readonly IReadOnlyDictionary<string, FileExtension> supportedExtension = new ReadOnlyDictionary<string, FileExtension>(new Dictionary<string, FileExtension>
        {
            { Wav.Extension, Wav }
        });

        private FileExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentException(nameof(extension));
            }

            if (!ValidFileExtensionPattern.IsMatch(extension))
            {
                throw new ArgumentException($"Invalid file extension: {extension}");
            }

            Extension = extension.ToLowerInvariant();
        }

        public string Extension { get; }

        public bool Equals(FileExtension other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Extension == other.Extension;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((FileExtension)obj);
        }

        public override int GetHashCode()
        {
            return (Extension != null ? Extension.GetHashCode() : 0);
        }

        public static bool operator ==(FileExtension left, FileExtension right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileExtension left, FileExtension right)
        {
            return !Equals(left, right);
        }

        public static FileExtension Parse(string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                throw new ArgumentException(nameof(fileExtension));
            }

            if (!fileExtension.StartsWith("."))
            {
                fileExtension = "." + fileExtension;
            }

            try
            {
                string lowerCaseFileExtension = fileExtension.ToLowerInvariant();

                return supportedExtension.ContainsKey(lowerCaseFileExtension)
                    ? supportedExtension[lowerCaseFileExtension]
                    : new FileExtension(lowerCaseFileExtension);
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException($"File extension not supported {fileExtension}");
            }
        }
    }
}
