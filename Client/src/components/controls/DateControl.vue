<template>
  <v-flex
    xs12 sm6
    class="pa-2"
  >
    <v-dialog
      ref="datePicker"
      v-model="datePicker"
      :return-value.sync="model.value"
      width="290px"
    >
      <template v-slot:activator="{ on }">
        <span v-if="description">
          {{ $t(description) }}
          <i v-if="!required">({{ $t('core.optional') }})</i>
        </span>
        <v-text-field
          v-on="on"
          :value="getFormattedDate"
          :label="label ? $t(label) : false"
          :rules="getRules"
          readonly
          filled
          required
          @click="opened"
        />
      </template>
      <v-date-picker
        v-if="datePicker"
        v-model="date"
        scrollable
      >
        <v-btn
          text
          v-t="'core.cancel'"
          @click="datePicker = false"
        />
        <v-spacer />
        <v-btn
          text
          color="primary"
          v-t="'core.ok'"
          @click="$refs.datePicker.save(date)"
        />
      </v-date-picker>
    </v-dialog>

  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import UserModule from '../../store/users'

@Component
export default class DateControl extends Vue {
  private userModule = getModule(UserModule, this.$store)

  @Prop(Object)
  private readonly model?: { value: string }

  @Prop(String)
  private readonly label?: string

  @Prop(String)
  private readonly description?: string

  @Prop(Object)
  private readonly customRules?: any[]

  @Prop(Boolean)
  private required: boolean = false

  private datePicker: any = false
  private date: string = ''
  private rules = [
    (v: string) => !this.required
                   || !!v
                   || i18n.t('core.fieldRequired'),
  ]

  private get getFormattedDate() {
    if (this.model && this.userModule.getUser) {
      return this.userModule.getUser.formatDate(this.model.value)
    }

    return this.model?.value || ''
  }

  private get getRules() {
    if (this.customRules) {
      return this.rules.concat(this.customRules)
    }
    return this.rules
  }

  private opened() {
    this.date = this.model?.value || ''
  }

}
</script>
