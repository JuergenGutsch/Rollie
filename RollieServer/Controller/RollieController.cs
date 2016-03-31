using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using RollieServer.Machine;

namespace RollieServer.Controller
{
    [RestController(InstanceCreationType.Singleton)]
    public class RollieController
    {
        private readonly RollieMachine _rollie;
        public RollieController()
        {
            _rollie = new RollieMachine(
                leftEngine: new Engine(forwardPin: 23, backwardPin: 22),
                rightEngine: new Engine(forwardPin: 17, backwardPin: 18));
        }

        [UriFormat("/rollie/{wheel}/{direction}/{rnd}")]
        public GetResponse Wheel(string wheel, string direction, string rnd)
        {
            var stateChanged = false;

            if (wheel.Equals("left"))
            {
                _rollie.LeftState = EngineState.Off;

                if (direction.Equals("forward"))
                {
                    _rollie.LeftState = EngineState.ForwardOn;
                    stateChanged = true;
                }
                else if (direction.Equals("backward"))
                {
                    _rollie.LeftState = EngineState.BackwardOn;
                    stateChanged = true;
                }
                else if (direction.Equals("off"))
                {
                    _rollie.LeftState = EngineState.Off;
                    stateChanged = true;
                }
            }

            if (wheel.Equals("right"))
            {
                _rollie.RightState = EngineState.Off;
                if (direction.Equals("forward"))
                {
                    _rollie.RightState = EngineState.ForwardOn;
                    stateChanged = true;
                }
                else if (direction.Equals("backward"))
                {
                    _rollie.RightState = EngineState.BackwardOn;
                    stateChanged = true;
                }
                else if (direction.Equals("off"))
                {
                    _rollie.RightState = EngineState.Off;
                    stateChanged = true;
                }
            }

            if (stateChanged)
            {
                _rollie.Move();
            }

            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new DataReceived { Wheel = wheel, Direction = direction });
        }

        [UriFormat("/rollie/camera")]
        public async Task<GetResponse> Camera()
        {
            var mediaCapture = new MediaCapture();
            await mediaCapture.InitializeAsync();

            var stream = new InMemoryRandomAccessStream();
            await mediaCapture.StartRecordToStreamAsync(
                     MediaEncodingProfile.CreateMp3(AudioEncodingQuality.Medium),
                     stream);

            var response = new GetResponse(
                GetResponse.ResponseStatus.OK, stream);

            return response;
        }
    }

    public sealed class DataReceived
    {
        public string Wheel { get; set; }
        public string Direction { get; set; }
    }
}