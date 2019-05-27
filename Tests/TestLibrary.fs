module TestLibrary

open Shouldly
open Consilium
open CommonTypes

let shouldSucceedWith (result: 'a) (value: Result<'a, Error list>) =
    match value with
    | Ok x -> x.ShouldBe result
    | Error x -> raise (System.Exception <| x.Head.ToString())

let shouldSucceedDoing resultFunc (value: Result<'a, Error list>) =
    match value with
    | Ok x -> (resultFunc x).ShouldBe true
    | Error x -> raise (System.Exception <| x.Head.ToString())

let shouldContainError (error: Error) (value: Result<'a, Error list>) =
    match value with
    | Ok _ -> raise <| System.Exception "Should throw error"
    | Error x ->
        x.ShouldContain error
        true
