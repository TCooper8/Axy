// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Try.fs"
#load "Utils.fs"
#load "Async.fs"
#load "Actor.fs"

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
