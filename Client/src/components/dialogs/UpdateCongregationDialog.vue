<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        icon
      >
        <v-icon>edit</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'project.congregation.update'"
          />
        </v-card-title>
        <v-card-text>

          <p
            class="subtitle-1"
            v-t="'project.congregation.updateDescription'"
          />

          <p v-t="'project.congregation.nameDescription'" />
          <v-text-field
            v-model="name"
            :label="$t('core.name')"
            :rules="nameRules"
            filled
            required
          />

          <p v-t="'project.congregation.numberDescription'" />
          <v-text-field
            v-model="number"
            :label="$t('project.congregation.number')"
            :rules="numberRules"
            filled
            required
          />

        </v-card-text>
        <v-card-actions>
          <DeleteCongregationDialog
            v-if="canBeDeleted"
            :congregation="congregation"
          />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-spacer />
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
import { VForm } from 'vuetify/lib'
import i18n from '../../i18n'
import AlertModule from '../../store/alerts'
import CongregationModule from '../../store/congregations'
import { Congregation, Exceptions } from '../../models'
import DeleteCongregationDialog from './DeleteCongregationDialog.vue'

@Component({
  components: {
    DeleteCongregationDialog,
  },
})
export default class UpdateCongregationDialog extends Vue {
  private alertModule: AlertModule = getModule(AlertModule, this.$store)
  private congregationModule: CongregationModule = getModule(CongregationModule, this.$store)

  @Prop(Congregation)
  private readonly congregation?: Congregation

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 2 || i18n.t('core.fieldMin', { count: 2 })
  ]
  private number: string = ''
  private numberRules: any[] = [
    (v: string) => v.length <= 10 || i18n.t('core.fieldMax', { count: 10 }),
  ]

  private get canBeDeleted() {
    return this.congregation?.numberOfParticipants === 0
  }

  private created() {
    if (this.congregation) {
      this.name = this.congregation.name
      this.number = this.congregation.number
    }
  }

  private async save() {
    if (this.congregation) {
      this.loading = true

      const response = await this.congregationModule.updateCongregation({
        congregationId: this.congregation.id,
        name: this.name,
        number: this.number,
      })
      if (response === Exceptions.CongregationNameUnique) {
        const form: any = this.$refs.form
        const thisName = this.name
        this.nameRules.push((v: string) => v !== thisName || i18n.t('project.congregation.nameUnique'))
        form.validate()
      } else if (response === Exceptions.CongregationNumberUnique) {
        const form: any = this.$refs.form
        const thisNumber = this.number
        this.numberRules.push((v: string) => v !== thisNumber || i18n.t('project.congregation.numberUnique'))
        form.validate()
      } else if (response) {
        this.dialog = false
      } else {
        this.alertModule.showAlert({
          message: i18n.t('core.generalError').toString(),
          color: 'error',
          timeout: 5000,
        })
      }

      this.loading = false
    }
  }

}
</script>
