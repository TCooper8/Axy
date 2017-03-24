namespace Axiom

module Async =
  let inline map mapping task = async {
    let! value = task
    return mapping value
  }

  let inline bind binding task = async {
    let! value = task
    return! binding value
  }

  let inline tap action task = async {
    let! value = task
    do action value
    return value
  }

  let inline iter action task = async {
    let! value = task
    do action value
  }

  let inline delay action = async {
    return action ()
  }
