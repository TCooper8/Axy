namespace Axiom

open Axiom

type 'a Actor = 'a MailboxProcessor

module Actor =
  type 'a ActorEvent =
    | Notify of 'a
    | DeathWatch
    | Kill
    | LastMessage

  type 'a ActorState =
    | Running of 'a
    | Failed of 'a * exn
    | Stopped

  type ActorBox<'msg, 'state> = {
    initialState: 'state
    onFailed: 'state -> exn -> 'state ActorState
    receive: 'state -> 'msg ActorEvent -> 'state ActorState
  }

  type Cmd =
    | Post of int

  let actorOf actorBox =
    MailboxProcessor.Start(fun inbox ->
      let rec loop = function
        | Running state ->
          inbox.Receive ()
          |> Async.map (fun cmd ->
            try actorBox.receive state cmd
            with e -> Failed (state, e)
          )
          |> Async.bind loop

        | Failed (state, e) ->
          printfn "Recovering actor..."
          try
            actorBox.onFailed state e
            |> loop
          with e ->
            actorBox.onFailed state e
            |> loop

        | Stopped ->
          printfn "Actor stopped."
          async.Zero ()

      actorBox.initialState |> Running |> loop
    )
