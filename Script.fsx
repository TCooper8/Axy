#load "src/Try.fs"
#load "src/Utils.fs"
#load "src/Async.fs"
#load "src/Actor.fs"

open System

open Axiom

module ActorTest =
  let actor: (int AsyncReplyChannel * int) Actor.ActorEvent Actor = Actor.actorOf {
    initialState = 5
    onFailed = fun state e ->
      sprintf "Error: %A" e
      Actor.Running state
    receive = fun x -> function
      | Actor.Notify (chan, y) ->
        chan.Reply(x)
        Actor.Running y
  }

  for i in 0 .. 1000000 do
    let x = actor.PostAndReply(fun reply -> Actor.Notify (reply, i))
    printfn "Got: %A" x

  Console.ReadKey () |> ignore
