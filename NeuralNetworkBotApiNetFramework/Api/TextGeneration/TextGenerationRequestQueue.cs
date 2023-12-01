using System;
using System.Collections.Generic;
using NeuralNetworkBotApiNetFramework.Api.TextGeneration.Data;

namespace NeuralNetworkBotApiNetFramework.Api.TextGeneration
{
    public class TextGenerationRequestQueue
    {
        private static readonly TextGenerationWebRequest Request = new();
        private readonly Queue<ApiRequest> _requestsQueue = new();
        private readonly Queue<ApiRequest> _notCompletedQueue = new();
        
        private bool _isMessageIsComputing;

        public void AddRequestInQueue(ApiRequest apiRequest)
        {
            _requestsQueue.Enqueue(apiRequest);
            ExecuteNextRequest();
        }
        
        private void ExecuteNextRequest()
        {
            if (!_isMessageIsComputing && _requestsQueue.Count > 0)
            {
                ComputeRequestInternal(_requestsQueue.Dequeue());
                return;
            }

            if (!_isMessageIsComputing && _notCompletedQueue.Count > 0)
                ComputeRequestInternal(_notCompletedQueue.Dequeue());
        }

        private async void ComputeRequestInternal(ApiRequest apiRequest)
        {
            _isMessageIsComputing = true;
            
            string resultMessage = apiRequest.Input;
            
            if (!string.IsNullOrEmpty(resultMessage))
            {
                History history = apiRequest.Bot.GetUserData(apiRequest.Sender).History;
                history.AddPromt(resultMessage);

                var result = await Request.Send(apiRequest);
                
                if (!string.IsNullOrEmpty(result))
                {
                    history.SetModelMessage(result);
                    apiRequest.OnRequestCompleted(result);
                    OnComputingComplete();
                }
                else
                {
                    history.RemoveLast();
                    _notCompletedQueue.Enqueue(apiRequest);
                    OnComputingComplete();
                }
            }
            else
            {
                apiRequest.OnRequestCompleted(apiRequest.Input);
                OnComputingComplete();
            }
        }

        private void OnComputingComplete()
        {
            _isMessageIsComputing = false;
            ExecuteNextRequest();
        }
    }
}