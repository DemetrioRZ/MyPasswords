using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Windows.Documents;
using Model;

namespace Views
{
    public class FlowDocumentCreator
    {
        [SecurityCritical]
        public FlowDocument Create(IEnumerable<Account> accounts)
        {
            var flowDocument = new FlowDocument();

            var groups = accounts.GroupBy(x => x.AccountType);

            foreach (var group in groups)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Italic(new Bold(new Run(group.Key))));
                paragraph.Inlines.Add(new LineBreak());
                paragraph.Inlines.Add(new LineBreak());
                
                foreach(var account in group)
                {
                    paragraph.Inlines.Add(new Bold(new Run($"{account.ResourceName}")));
                    paragraph.Inlines.Add(new LineBreak());
                    paragraph.Inlines.Add(new Run("Account: "));
                    paragraph.Inlines.Add(new Bold(new Run($"{account.Login}")));
                    paragraph.Inlines.Add(new Run(" -> password: "));
                    paragraph.Inlines.Add(new Bold(new Run($"{account.Password.ToUnsecure()}")));
                    paragraph.Inlines.Add(new LineBreak());
                    paragraph.Inlines.Add(new Run($"{account.Comment}"));
                    paragraph.Inlines.Add(new LineBreak());
                    paragraph.Inlines.Add(new LineBreak());
                }

                flowDocument.Blocks.Add(paragraph);
            }
            
            return flowDocument;
        }
    }
}