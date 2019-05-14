namespace Consilium

open CommonLibrary

module Validation =

    let (&&&) v1 v2 = 
        let addSuccess r1 r2 = r1 // return first
        let addFailure s1 s2 = s1 @ s2  // concat
        plus addSuccess addFailure v1 v2 

