<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-list-item v-on="on">
        <v-list-item-title v-t="'shift.create'" />
      </v-list-item>
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'shift.create'"
          />
        </v-card-title>

        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'shift.createDescription'"
          />
          <p
            class="subtitle-1"
            v-t="'shift.nameDescription'"
          />
          <v-text-field
            v-model="name"
            :label="$t('core.name')"
            :rules="nameRules"
            filled
            required
          />

        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid"
            type="submit"
            :loading="loading"
            text
            color="primary"
            @click.stop="save"
            v-t="'core.save'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import RoleModule from '../../store/roles'

@Component
export default class CreateShiftDialog extends Vue {
  private roleModule: RoleModule = getModule(RoleModule, this.$store)

  @Prop(Object)
  private readonly date: any

  @Prop(Object)
  private menu: any

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 2 || i18n.t('core.fieldMin', { count: 2 })
  ]

  private async save() {
    if (this.valid) {
      this.loading = true

      // await this.roleModule.createRole({
      //   name: this.name,
      // })

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
