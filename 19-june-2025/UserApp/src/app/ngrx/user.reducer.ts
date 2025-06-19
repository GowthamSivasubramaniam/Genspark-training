import { createReducer,on} from "@ngrx/store";
import { initialUserState } from "./UserState";
import * as UserActions from "./users.actions";
import { User } from "../models/User";


export const userReducer = createReducer(initialUserState,
    on(UserActions.loadUsersSuccess, (state,{users})=>({...state, users, loading: false, error: null })),
    on(UserActions.loadUsersFailure, (state,{error})=>({...state, loading: false, error })),
    on(UserActions.addUser, (state,{user})=>({...state, users: [...state.users, user], loading: false, error: null })),
)