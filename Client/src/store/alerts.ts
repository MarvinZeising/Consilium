import { Module, VuexModule, Mutation, Action, MutationAction } from 'vuex-module-decorators'
import store from '../plugins/vuex'

@Module({ dynamic: true, store, name: 'AlertModule' })
export default class AlertModule extends VuexModule {
  public loading: boolean = true
  public snackbar: any = {}

  public get getSnackbar(): any {
    return this.snackbar
  }

  @Action
  public updateLoading(loading: boolean) {
    this.context.commit('setLoading', loading)
  }

  @MutationAction({ mutate: ['snackbar'] })
  public async showAlert({ message, color, timeout }: {
    message: string,
    color?: string,
    timeout?: number,
  }) {
    return {
      snackbar: {
        show: true,
        color: color || '',
        y: 'bottom',
        x: null,
        mode: '',
        timeout: timeout || 0,
        text: message || 'Error'
      }
    }
  }

  @Mutation
  private setLoading(loading: boolean) {
    this.loading = loading
  }

}
