using System;
using System.Collections.Generic;
using NeuralNetworkBotApiNetFramework.Api.TextGeneration.Data;

namespace NeuralNetworkBotApiNetFramework.Api.TextGeneration
{
    public class TextGenerationRequestQueue
    {
        private static readonly TextGenerationWebRequest Request = new();
        private readonly Queue<ApiRequest> _requestsQueue = new();
        
        private bool _isMessageIsComputing;

        public void AddRequestInQueue(ApiRequest apiRequest)
        {
            _requestsQueue.Enqueue(apiRequest);
            ExecuteNextRequest();
        }
        
        private void ExecuteNextRequest()
        {
            if(!_isMessageIsComputing && _requestsQueue.Count>0)
                ComputeRequestInternal(_requestsQueue.Dequeue(), ExecuteNextRequest);
        }

        private async void ComputeRequestInternal(ApiRequest apiRequest, Action onComplete)
        {
            _isMessageIsComputing = true;
            
            string resultMessage = apiRequest.Input;
            
            if (!string.IsNullOrEmpty(resultMessage))
            {
                History history = apiRequest.Bot.GetUserHistory(apiRequest.Sender);
                history.AddPromt(resultMessage);

                var result = await Request.Send(apiRequest);

                if (!string.IsNullOrEmpty(result))
                {
                    history.SetModelMessage(result);
                    apiRequest.OnRequestCompleted(result);
                    _isMessageIsComputing = false;
                    
                    onComplete?.Invoke();
                }
                else
                {
                    history.RemoveLast();
                    ComputeRequestInternal(apiRequest, onComplete);
                }
            }
            else
            {
                _isMessageIsComputing = false;
                apiRequest.OnRequestCompleted(apiRequest.Input);
                onComplete?.Invoke();
            }
        }
    }
}