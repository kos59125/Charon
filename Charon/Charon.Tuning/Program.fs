﻿open System
open Charon.Entropy
open Charon.MDL
open Charon.Tree

[<EntryPoint>]
let main argv = 
    printfn "Started"

    let size = 100000
    let classes = 3

    let rng = Random(42)

    let outcomes = [| for i in 1 .. size -> rng.Next(classes) |]

    let features = 
        [|  yield outcomes |> Array.map (fun x -> (if x = 0 then Some(rng.NextDouble()) else Some(rng.NextDouble() + 1.)), x) |> Numeric;
            yield outcomes |> Array.map (fun x -> (if x = 2 then Some(rng.NextDouble()) else Some(rng.NextDouble() + 1.)), x) |> Numeric;
            yield outcomes |> Array.map (fun x -> Some(rng.NextDouble()), x) |> Numeric;
            yield outcomes |> Array.map (fun x -> if x = 0 then 0 else 1) |> Charon.Discrete.prepare |> Categorical; |]

    let dataset = { Classes = classes; Outcomes = outcomes; Features = features }
    let filter = [| 0 .. (size - 1) |]
    let remaining = [ 0; 1; ] |> Set.ofList
    let selector = id

    let tree = growTree dataset filter remaining selector 5

    0 // return an integer exit code
