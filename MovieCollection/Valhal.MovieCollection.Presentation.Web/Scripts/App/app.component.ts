import { Component } from '@angular/core';
@Component({
    selector: 'my-app',
    template: '<h1>My First Angular App - Demo version IS {{XXX}} </h1>'
})
export class AppComponent {
    XXX = "1.0";
    constructor() {
        console.log("TESTXXXME");
    }
}