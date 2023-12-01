using System.Collections.Generic;
using MLBotApiNetFramework.Bots;
using Newtonsoft.Json;

namespace MLBotApiNetFramework.Api.TextGeneration.Data
{
    internal class RequestData
    {
        public IReadOnlyDictionary<string, object> Data => _data;
        private readonly Dictionary<string, object> _data;

        public RequestData(string message, BotUserData userData, BotConfig config, History history = null)
        {
            History historyResult = history ?? new History();

            _data = new Dictionary<string, object>()
            {
                {"user_input", message},
                {"max_new_tokens", 1000},
                {"history", historyResult},
                {"mode", "chat"},
                {"character", config.Name},
                {"instruction_template", "Vicuna-v1.1"},
                {"your_name", userData.Name},
                {"regenerate", false},
                {"_continue", history == null},
                {"stop_at_newline", false},
                {"chat_generation_attempts", 1},
                {"chat-instruct_command", ""},
                {"context", string.IsNullOrEmpty(userData.UserContext) ? config.Context : userData.UserContext},
                {"preset", "simple-1"},
                {"do_sample", true},
                {"temperature", 0.7},
                {"top_p", 0.1},
                {"typical_p", 1},
                {"epsilon_cutoff", 0},  
                {"eta_cutoff", 0},  
                {"tfs", 1},
                {"top_a", 0},
                {"repetition_penalty", 1.18},
                {"repetition_penalty_range", 0},
                {"top_k", 40},
                {"min_length", 0},
                {"no_repeat_ngram_size", 0},
                {"num_beams", 1},
                {"penalty_alpha", 0},
                {"length_penalty", 1},
                {"early_stopping", false},
                {"mirostat_mode", 0},
                {"mirostat_tau", 5},
                {"mirostat_eta", 0.1},

                {"seed", -1},
                {"add_bos_token", true},
                {"truncation_length", 2048},
                {"ban_eos_token", false},
                {"skip_special_tokens", true},
                {"stopping_strings", config.StoppingStrings}
            };
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_data);
        }
    }
}