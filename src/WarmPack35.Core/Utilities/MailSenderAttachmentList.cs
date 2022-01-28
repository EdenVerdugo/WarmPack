using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WarmPack.Utilities
{
    public class MailSenderAttachmentList : IList<MailSenderAttachment>
    {
        private List<MailSenderAttachment> _attachmentList;
        public MailSenderAttachmentList()
        {
            _attachmentList = new List<MailSenderAttachment>();
        }

        public MailSenderAttachment this[int index] { get => _attachmentList[index]; set => _attachmentList[index] = value; }

        public int Count => _attachmentList.Count();

        public bool IsReadOnly => false;

        public void Add(MailSenderAttachment item)
        {
            _attachmentList.Add(item);
        }

        public void Add(string name, byte[] bufferArray, string mimeType = "")
        {
            var item = new MailSenderAttachment(name, bufferArray, mimeType);

            _attachmentList.Add(item);
        }

        public void Add(string name, Stream stream, string mimeType = "")
        {
            var item = new MailSenderAttachment(name, stream, mimeType);

            _attachmentList.Add(item);
        }

        public void Add(string path, string mimeType = "")
        {
            var item = new MailSenderAttachment(path, mimeType);

            _attachmentList.Add(item);
        }

        public void Clear()
        {
            _attachmentList.Clear();
        }

        public bool Contains(MailSenderAttachment item)
        {
            return _attachmentList.Contains(item);
        }

        public void CopyTo(MailSenderAttachment[] array, int arrayIndex)
        {
            _attachmentList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MailSenderAttachment> GetEnumerator()
        {
            return _attachmentList.GetEnumerator();
        }

        public int IndexOf(MailSenderAttachment item)
        {
            return _attachmentList.IndexOf(item);
        }

        public void Insert(int index, MailSenderAttachment item)
        {
            _attachmentList.Insert(index, item);
        }

        public bool Remove(MailSenderAttachment item)
        {
            return _attachmentList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _attachmentList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _attachmentList.GetEnumerator();
        }
    }
}
