namespace Facebook.Unity
{
    using System.Collections.Generic;

    internal class GetCurrentGamingContextResult : ResultBase, IGetCurrentGamingContextResult
    {
        internal GetCurrentGamingContextResult(ResultContainer resultContainer) : base(resultContainer)
        {
            if (this.ResultDictionary != null)
            {
                string contextId;
                if (this.ResultDictionary.TryGetValue<string>("contextId", out contextId))
                {
                    this.ContextId = contextId;
                }
            }
        }

        public string ContextId { get; private set; }

        public override string ToString()
        {
            return Utilities.FormatToString(
                base.ToString(),
                this.GetType().Name,
                new Dictionary<string, string>()
                {
                    { "ContextId", this.ContextId },
                });
        }
    }


}
