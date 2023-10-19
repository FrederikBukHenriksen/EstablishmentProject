import { Observable } from "rxjs/internal/Observable";

export async function ObservableToPromise<T>(obs: Observable<T>, error: Error = new Error()) : Promise<T> {
    const res = await obs.toPromise()
        .then((res) => {
            return res;
        }).catch((error) => {
            throw error;
        })
        if (res) {
            return res;
        }
        throw error;
}