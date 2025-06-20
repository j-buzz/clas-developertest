// This file is required by karma.conf.js and loads recursively all the .spec and framework files

import 'zone.js/testing';
import {getTestBed} from '@angular/core/testing';
import {BrowserDynamicTestingModule, platformBrowserDynamicTesting} from '@angular/platform-browser-dynamic/testing';
import {toHaveNoViolations} from "jasmine-axe";
import * as axe from "axe-core";

// First, initialize the Angular testing environment.
getTestBed().initTestEnvironment(
    BrowserDynamicTestingModule,
    platformBrowserDynamicTesting(),
);

beforeAll(() => {
    axe.configure({})
    jasmine.addMatchers(toHaveNoViolations)
})
