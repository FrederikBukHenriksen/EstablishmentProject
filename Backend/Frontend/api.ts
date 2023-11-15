//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class AnalysisClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    productSalesChart(command: GetProductSalesChartQuery): Observable<LineChartData> {
        let url_ = this.baseUrl + "/api/analysis/sales-line-chart";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(command);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processProductSalesChart(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processProductSalesChart(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<LineChartData>;
                }
            } else
                return _observableThrow(response_) as any as Observable<LineChartData>;
        }));
    }

    protected processProductSalesChart(response: HttpResponseBase): Observable<LineChartData> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as LineChartData;
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }
}

@Injectable({
    providedIn: 'root'
})
export class AuthenticationClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    logIn(loginCommand: LoginCommand): Observable<void> {
        let url_ = this.baseUrl + "/api/authentication/login";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(loginCommand);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processLogIn(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processLogIn(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processLogIn(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return _observableOf(null as any);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    logOut(): Observable<void> {
        let url_ = this.baseUrl + "/api/authentication/logout";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processLogOut(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processLogOut(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processLogOut(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return _observableOf(null as any);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    isLoggedIn(): Observable<boolean> {
        let url_ = this.baseUrl + "/api/authentication/is-logged-in";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processIsLoggedIn(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processIsLoggedIn(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<boolean>;
                }
            } else
                return _observableThrow(response_) as any as Observable<boolean>;
        }));
    }

    protected processIsLoggedIn(response: HttpResponseBase): Observable<boolean> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as boolean;
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    getLoggedInUser(): Observable<User> {
        let url_ = this.baseUrl + "/api/authentication/get-user-info";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetLoggedInUser(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetLoggedInUser(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<User>;
                }
            } else
                return _observableThrow(response_) as any as Observable<User>;
        }));
    }

    protected processGetLoggedInUser(response: HttpResponseBase): Observable<User> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as User;
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }
}

@Injectable({
    providedIn: 'root'
})
export class EstablishmentClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    get(establishmentId: string | undefined): Observable<Establishment> {
        let url_ = this.baseUrl + "/api/establishment/get?";
        if (establishmentId === null)
            throw new Error("The parameter 'establishmentId' cannot be null.");
        else if (establishmentId !== undefined)
            url_ += "establishmentId=" + encodeURIComponent("" + establishmentId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGet(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGet(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<Establishment>;
                }
            } else
                return _observableThrow(response_) as any as Observable<Establishment>;
        }));
    }

    protected processGet(response: HttpResponseBase): Observable<Establishment> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as Establishment;
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }

    getAll(): Observable<Establishment[] | null> {
        let url_ = this.baseUrl + "/api/establishment/get-all";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAll(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<Establishment[] | null>;
                }
            } else
                return _observableThrow(response_) as any as Observable<Establishment[] | null>;
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<Establishment[] | null> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as Establishment[];
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }
}

@Injectable({
    providedIn: 'root'
})
export class SalesClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    getPoductSales(): Observable<void> {
        let url_ = this.baseUrl + "/api/sales/product";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetPoductSales(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetPoductSales(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processGetPoductSales(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return _observableOf(null as any);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }
}

@Injectable({
    providedIn: 'root'
})
export class TestClient {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    establishment(): Observable<User> {
        let url_ = this.baseUrl + "/api/test";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            withCredentials: true,
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processEstablishment(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processEstablishment(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<User>;
                }
            } else
                return _observableThrow(response_) as any as Observable<User>;
        }));
    }

    protected processEstablishment(response: HttpResponseBase): Observable<User> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as User;
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(null as any);
    }
}

export interface Chart {
    type: ChartType;
}

export interface LineChartData extends Chart {
    xLegend: string | undefined;
    yLegend: string | undefined;
    values: ValueTupleOfStringAndInteger[];
}

export interface ValueTupleOfStringAndInteger {
    item1: string;
    item2: number;
}

export enum ChartType {
    LineChart = 0,
    BarChart = 1,
    PieChart = 2,
}

export interface GetProductSalesChartQuery {
    productId: string[];
    startDate: Date;
    endDate: Date;
}

export interface LoginCommand {
    username: string;
    password: string;
}

export interface EntityBase {
    id: string;
}

export interface User extends EntityBase {
    username: string;
    password: string;
    userRoles: UserRole[];
}

export interface UserRole extends EntityBase {
    user: User;
    establishment: Establishment;
    role: Role;
}

export interface Establishment extends EntityBase {
    name: string;
    location: Location | undefined;
    tables: Table[] | undefined;
    items: Item[] | undefined;
    sales: Sale[] | undefined;
}

export interface Location extends EntityBase {
    country: string;
}

export interface Table extends EntityBase {
    establishment: Establishment;
    name: string;
}

export interface Item extends EntityBase {
    name: string;
    price: number;
}

export interface Sale extends EntityBase {
    establishment: Establishment;
    timestamp: Date;
    items: Item[];
    table: Table | undefined;
}

export enum Role {
    Admin = 0,
    User = 1,
}

export class ApiException extends Error {
    override message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new ApiException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((event.target as any).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}